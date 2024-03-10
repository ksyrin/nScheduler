using nScheduler.Domain.Models.Jobs;
using System.Linq.Expressions;

namespace nScheduler.Domain.Repositories.Jobs;

public interface IJobLogRepository : IBaseRepository<JobLogModel, Guid>
{
    Task<List<JobLogModel>> GetJobisRunning(CancellationToken cancellationToken);

    Task<Dictionary<string, int>> GetJobLogGroupByDay(Expression<Func<JobLogModel, bool>>? expression);
}