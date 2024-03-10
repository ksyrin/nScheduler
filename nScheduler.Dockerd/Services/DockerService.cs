using nScheduler.Dockerd.Events;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Services;

namespace nScheduler.Dockerd.Services;

public class DockerService : IJobService
{
    private readonly DockerEvent docker;

    public DockerService(DockerEvent docker)
    {
        this.docker = docker;
    }

    public async Task UpdateJob(List<JobLogModel> models)
    {
        foreach (var model in models)
        {
            await docker.UpdateState(model);
        }
    }

    public async Task<List<JobLogModel>> StartJobs(List<JobInfoModel> models)
    {
        var logs = new List<JobLogModel>();
        foreach (var model in models)
        {
            //await docker.PullImage(model.Image.ImageName);
            //var log = await docker.StartJob(model);
            //logs.Add(log);
        }

        return logs;
    }

    public Task RemoveJob(Guid Id)
    {
        return docker.RemoveJob(Id);
    }

    public Task RemoveJobs(List<JobLogModel> models)
    {
        var finishJobs = models.Where(x => x.IsFinish).ToList();
        foreach (var job in finishJobs)
        {
            RemoveJob(job.Id);
            models.Remove(job);
        }
        return Task.CompletedTask;
    }
}