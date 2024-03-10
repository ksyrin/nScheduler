using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Domain.Repositories.Jobs;

public interface IJobInfoRepository : IBaseRepository<JobInfoModel, Guid>
{
    /// <summary>
    /// 统计作业类型的数量
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<Dictionary<string, int>> GetJobTypeGroupByDay(CancellationToken cancellationToken = default);

    /// <summary>
    /// 将作业设置为结束
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task SetJobFinish(JobInfoModel model, DateTime execTime, CancellationToken cancellationToken = default);
}