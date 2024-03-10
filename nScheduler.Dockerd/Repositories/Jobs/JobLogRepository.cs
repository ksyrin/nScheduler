using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Repositories.Jobs;

namespace nScheduler.Dockerd.Repositories.Jobs;

public class JobLogRepository : BaseRepository<JobLogModel>, IJobLogRepository
{
    public JobLogRepository(DataContext context) : base(context)
    {
    }

    public Task<List<JobLogModel>> GetJobisRunning()
    {
        return context.Set<JobLogModel>().Where(x => x.JobState == "running").ToListAsync();
    }
}