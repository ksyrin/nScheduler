using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Repositories.Jobs;
using System.Linq.Expressions;

namespace nScheduler.Imp.Repositories.Jobs;

public class JobInfoRepository : BaseRepository<JobInfoModel, Guid>, IJobInfoRepository
{
    public JobInfoRepository(DataContext context) : base(context)
    {
    }

    public override async ValueTask<JobInfoModel?> Find(Guid Id, CancellationToken cancellationToken = default)
    {
        return await context.Set<JobInfoModel>()
             .Include(x => x.Image)
             .FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
    }

    public override Task<List<JobInfoModel>> List(Expression<Func<JobInfoModel, bool>>? expression, int page, int size, CancellationToken cancellationToken = default)
    {
        return context.Set<JobInfoModel>()
            .Include(x => x.Image)
            .OrderByDescending(x => x.OperTime)
            .Where(expression ?? (x => true))
            .Skip((page - 1) * size).Take(size)
            .ToListAsync(cancellationToken);
    }

    public async Task SetJobFinish(JobInfoModel model, DateTime execTime, CancellationToken cancellationToken = default)
    {
        model.Status = 0;
        model.ExecTime = execTime;
        await Edit(model, cancellationToken);
    }

    public Task<Dictionary<string, int>> GetJobTypeGroupByDay(CancellationToken cancellationToken = default)
    {
        return context.Set<JobInfoModel>()
            .Include(x => x.Image)
            .Select(x => new { x.Image.ImageName })
            .GroupBy(x => x.ImageName)
            .ToDictionaryAsync(x => x.Key, x => x.Count(), cancellationToken);
    }
}