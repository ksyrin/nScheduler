using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobLogSearchViewModel : ISearchModel<JobLogModel>
{
    public string JobName { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Guid? ImageId { get; set; }

    public string Status { get; set; }

    public override void CreateSearchFunc()
    {
        if (!JobName.IsEmpty())
        {
            AddSearchAction(nameof(JobLogModel.JobName), JobName, SearchType.Like);
        }

        if (StartTime.HasValue)
        {
            AddSearchAction(nameof(JobLogModel.BeginTime), StartTime.Value, SearchType.GreaterThanOrEqual);
        }

        if (EndTime.HasValue)
        {
            AddSearchAction(nameof(JobLogModel.BeginTime), EndTime.Value, SearchType.LessThanOrEqual);
        }

        if (ImageId.HasValue)
        {
            AddSearchAction(nameof(JobLogModel.ImageId), ImageId);
        }

        if (!Status.IsEmpty())
        {
            AddSearchAction(nameof(JobLogModel.JobState), Status);
        }
    }
}