using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Events;
using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Exec.K8s
{
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
                await client.CreateNamespacedPodAsync(new k8s.Models.V1Pod
                {
                    Metadata = new k8s.Models.V1ObjectMeta
                    {
                        Name = id.ToStringN(),
                    },
                    Kind = "Pod",
                    ApiVersion = "v1",
                    Spec = new k8s.Models.V1PodSpec
                    {
                        Containers = new List<V1Container> {
                            {
                                new V1Container
                                {
                                    Image = model.Image.ImageName,
                                    Name = id.ToStringN(),
                                    Args = new string[] { cmds.ToJson() },
                                    Env = envs.Select(x => new V1EnvVar { Name = x.Key, Value = x.Value} ).ToList()
                                }
                            } }
                    }
                }, namespaceName, cancellationToken: cancellationToken);

                return new JobLogModel(id, model.ImageId, model.Name, DateTime.Now, JobStatus.Running, model);
            }
            catch (Exception ex)
            {
                throw new Exception("运行作业容器失败: " + ex.Message);
            }
        }

        public async Task UpdateState(JobLogModel model, CancellationToken cancellationToken = default)
        {
            var pod = await client.ReadNamespacedPodAsync(model.Id.ToStringN(), namespaceName);
            var status = pod.Status.Phase;
            model.UpdateStatus(status == "Completed" ? JobStatus.Completed : status == "Running" ? JobStatus.Running : JobStatus.Error);

            var result = await client.ReadNamespacedPodLogAsync(model.Id.ToStringN(), namespaceName);
            StreamReader sr = new StreamReader(result!);
            model.Content = await sr.ReadToEndAsync();
        }
    }
}