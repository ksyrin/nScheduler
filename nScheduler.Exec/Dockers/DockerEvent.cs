using Docker.DotNet;
using Docker.DotNet.Models;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Events;
using nScheduler.Domain.Models.Jobs;
using System.Text;
using System.Text.RegularExpressions;

namespace nScheduler.Exec.Dockers;

public class DockerEvent : ISchedulerEvent
{
    private readonly IDockerClient client;

    private List<string> Images { get; set; }

    public DockerEvent(IDockerClient client)
    {
        this.client = client;
        Images = new();
    }

    public async Task RemoveJob(Guid Id, CancellationToken cancellationToken = default)
    {
        try
        {
            await client.Containers.RemoveContainerAsync(Id.ToString("N"), new ContainerRemoveParameters()
            {
                Force = true
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("移除作业容器失败: " + ex.Message);
        }
    }

    private async Task PullImage(string imageName, CancellationToken cancellationToken = default)
    {
        // 获取镜像列表
        if (Images.Count == 0)
        {
            try
            {
                var list = await client.Images.ListImagesAsync(new ImagesListParameters()
                {
                    All = true
                }, cancellationToken);
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        if (i.RepoTags != null && i.RepoTags.Count > 0)
                            Images.AddRange(i.RepoTags);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("获取作业镜像列表失败: " + e.Message);
            }
        }
        // 拉取镜像
        if (!Images.Any(x => x.Contains(imageName)))
        {
            try
            {
                var imgd = imageName.Split(":");
                await client.Images.CreateImageAsync(new ImagesCreateParameters()
                {
                    FromImage = imgd[0],
                    Tag = imgd.Count() == 1 ? "latest" : imgd[1]
                }, null, new Progress<JSONMessage>(), cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception($"拉取 {imageName} 作业镜像失败:" + e.Message);
            }
        }
    }

    public async Task<JobLogModel> StartJob(JobInfoModel model, Dictionary<string, string> cmds, Dictionary<string, string> envs, CancellationToken cancellationToken = default)
    {
        await PullImage(model.Image.ImageName, cancellationToken);
        try
        {
            var id = Guid.NewGuid();
            // 创建容器
            await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Name = id.ToString("N"),
                Image = model.Image.ImageName,
                Cmd = new string[] { cmds.ToJson() },
                Env = envs.Select(x => x.Key + "=" + x.Value).ToList()
            }, cancellationToken);
            // 运行容器
            await client.Containers.StartContainerAsync(id.ToString("N"), new ContainerStartParameters(), cancellationToken);
            // 更新作业信息
            model.Status = 1;
            // 返回模型
            return new JobLogModel(id, model.ImageId, model.Name, DateTime.Now, Common.Models.JobStatus.Running, model);
        }
        catch (Exception ex)
        {
            throw new Exception("运行作业容器失败: " + ex.Message);
        }
    }

    public async Task UpdateState(JobLogModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = model.Id.ToStringN();
            var response = await client.Containers.InspectContainerAsync(id, cancellationToken);

            // 检查各作业的状态
            if (response != null)
            {
                model.UpdateStatus(response.State.Status == "exited" ? Common.Models.JobStatus.Completed : Common.Models.JobStatus.Running);
                var stream = await client.Containers.GetContainerLogsAsync(id, true, new ContainerLogsParameters
                {
                    ShowStdout = true,
                    ShowStderr = true
                }, cancellationToken);
                model.Content = await ReadOutputAsync(stream, cancellationToken);
            }
        }
        catch (Exception e)
        {
            throw new Exception("更新作业状态失败：" + e.Message);
        }
    }

    private static async Task<string> ReadOutputAsync(MultiplexedStream multiplexedStream, CancellationToken cancellationToken = default)
    {
        StringBuilder sb = new();
        byte[] buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(81920);

        while (true)
        {
            Array.Clear(buffer, 0, buffer.Length);

            MultiplexedStream.ReadResult readResult = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken);

            if (readResult.EOF)
            {
                break;
            }

            if (readResult.Count > 0)
            {
                var responseLine = Encoding.UTF8.GetString(buffer, 0, readResult.Count);
                var tmp = Regex.Replace(Regex.Replace(Regex.Replace(responseLine.Trim().ToString(), "\n", "nr"), @"[\p{C}]", ""), "nr", "\n\r");
                sb.AppendLine(tmp);
            }
            else
            {
                break;
            }
        }

        System.Buffers.ArrayPool<byte>.Shared.Return(buffer);

        return sb.ToString();
    }
}