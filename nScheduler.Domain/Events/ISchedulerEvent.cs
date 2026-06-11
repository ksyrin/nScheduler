using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Domain.Events;

public interface ISchedulerEvent
{
    /// <summary>
    /// 移除作业
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveJob(Guid Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 启动作业
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cmds"></param>
    /// <param name="envs"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JobLogModel> StartJob(JobInfoModel model, Dictionary<string, string> cmds, Dictionary<string, string> envs, CancellationToken cancellationToken = default);

    /// <summary>
    /// 实时获取作业日志输出
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<string> GetLogsAsync(Guid id, CancellationToken cancellationToken = default);
}