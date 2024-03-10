using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Repositories.Jobs;

namespace nScheduler.Dockerd.Repositories.Jobs;

public class JobInfoRepository : BaseRepository<JobInfoModel>, IJobInfoRepository
{
    public JobInfoRepository(DataContext context) : base(context)
    {
    }

    public override async ValueTask<JobInfoModel?> Find(Guid Id)
    {
        return await context.Set<JobInfoModel>()
             .Include(x => x.Image)
             .FirstOrDefaultAsync(x => x.Id == Id);
    }

    public override Task<IEnumerable<JobInfoModel>> List(Func<JobInfoModel, bool> expression, int page, int size)
    {
        return Task.FromResult(context.Set<JobInfoModel>()
            .Include(x => x.Image)
            .OrderByDescending(x => x.OperTime)
            .Where(expression == null ? x => true : expression)
            .Skip((page - 1) * size).Take(size));
    }

    public Task<List<JobInfoModel>> GetJobWillStart(int count)
    {
        return context.Set<JobInfoModel>().Where(x => x.Status == 0)
            .Take(count).ToListAsync();
    }

    public async Task UpdateJobState(List<JobInfoModel> models)
    {
        foreach (var model in models)
        {
            model.Status = 0;
            context.Set<JobInfoModel>().Update(model);
        }
        await context.SaveChangesAsync();
    }

    public async Task SetJobFinish(JobInfoModel model, DateTime execTime)
    {
        model.Status = 0;
        model.ExecTime = execTime;
        await Edit(model);
    }
}