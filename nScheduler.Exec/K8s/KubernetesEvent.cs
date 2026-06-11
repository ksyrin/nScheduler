using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Events;
using nScheduler.Domain.Models.Jobs;
using System.Runtime.CompilerServices;
using System.Text;

namespace nScheduler.Exec.K8s;

public class KubernetesEvent : ISchedulerEvent
{
    private readonly Kubernetes client;
    private readonly string namespaceName;

    public KubernetesEvent(Kubernetes client, IConfiguration configuration)
    {
        this.client = client;
        namespaceName = configuration.GetSection("client:namespace").Value!;
    }

    public async Task RemoveJob(Guid Id, CancellationToken cancellationToken = default)
    {
        await client.DeleteNamespacedPodAsync(Id.ToStringN(), namespaceName, cancellationToken: cancellationToken);
    }

    public async Task<JobLogModel> StartJob(JobInfoModel model, Dictionary<string, string> cmds, Dictionary<string, string> envs, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = Guid.NewGuid();
            await client.CreateNamespacedPodAsync(new V1Pod
            {
                Metadata = new V1ObjectMeta
                {
                    Name = id.ToStringN(),
                },
                Kind = "Pod",
                ApiVersion = "v1",
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container> {
                        new V1Container
                        {
                            Image = model.Image.ImageName,
                            Name = id.ToStringN(),
                            Args = new string[] { cmds.ToJson() },
                            Env = envs.Select(x => new V1EnvVar { Name = x.Key, Value = x.Value} ).ToList()
                        }
                    }
                }
            }, namespaceName, cancellationToken: cancellationToken);

            return new JobLogModel(id, model.ImageId, model.Name, DateTime.Now, JobStatus.Running, model);
        }
        catch (Exception ex)
        {
            throw new Exception("运行作业容器失败: " + ex.Message);
        }
    }

    public async IAsyncEnumerable<string> GetLogsAsync(Guid id, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var podName = id.ToStringN();

        // Ensure pod exists and is in a terminal state or still running
        using var stream = await client.ReadNamespacedPodLogAsync(
            podName,
            namespaceName,
            follow: true,
            previous: false,
            cancellationToken: cancellationToken
        );

        using var reader = new StreamReader(stream, Encoding.UTF8);
        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();
            if (line != null)
            {
                yield return line;
            }
        }
    }
}