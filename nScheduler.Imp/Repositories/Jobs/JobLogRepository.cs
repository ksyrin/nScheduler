using Microsoft.EntityFrameworkCore;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Repositories.Jobs;
using System.Linq.Expressions;

namespace nScheduler.Imp.Repositories.Jobs;

public class JobLogRepository : BaseRepository<JobLogModel, Guid>, IJobLogRepository
{
    public JobLogRepository(DataContext context) : base(context)
    {
    }

    public override Task<List<JobLogModel>> List(Expression<Func<JobLogModel, bool>>? expression, int page, int size, CancellationToken cancellationToken)
    {
        return context.Set<JobLogModel>()
            .OrderByDescending(x => x.BeginTime)
            .Where(expression ?? (x => true))
            .Skip((page - 1) * size).Take(size)
            .ToListAsync(cancellationToken);
    }

    public Task<List<JobLogModel>> GetJobisRunning(CancellationToken cancellationToken)
    {
        return context.Set<JobLogModel>().Where(x => x.JobState == JobStatus.Running).ToListAsync(cancellationToken);
    }

    public Task<Dictionary<string, int>> GetJobLogGroupByDay(Expression<Func<JobLogModel, bool>>? expression)
    {
        return Task.FromResult(context.Set<JobLogModel>()
            .OrderBy(x => x.BeginTime)
            .Where(expression?.Compile() ?? (x => true))
            .GroupBy(x => x.BeginTime.ToString("MMdd"))
            .ToDictionary(x => x.Key, x => x.Count()));
    }
}