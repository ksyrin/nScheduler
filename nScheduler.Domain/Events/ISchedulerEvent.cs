using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Domain.Events;

public interface ISchedulerEvent
{
    Task RemoveJob(Guid Id, CancellationToken cancellationToken = default);

    Task<JobLogModel> StartJob(JobInfoModel model, Dictionary<string, string> cmds, Dictionary<string, string> envs, CancellationToken cancellationToken = default);

    Task UpdateState(JobLogModel model, CancellationToken cancellationToken = default);
}