using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Jobs;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobInfoSearchViewModel : ISearchModel<JobInfoModel>
{
    public string Name { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Guid? ImageId { get; set; }

    public int? Status { get; set; }

    public override void CreateSearchFunc()
    {
        if (!Name.IsEmpty())
        {
            AddSearchAction(nameof(JobInfoModel.Name), Name, SearchType.Like);
        }

        if (StartTime.HasValue)
        {
            AddSearchAction(nameof(JobInfoModel.ExecTime), StartTime.Value, SearchType.GreaterThanOrEqual);
        }

        if (EndTime.HasValue)
        {
            AddSearchAction(nameof(JobInfoModel.ExecTime), EndTime.Value, SearchType.LessThanOrEqual);
        }

        if (ImageId.HasValue)
        {
            AddSearchAction(nameof(JobInfoModel.ImageId), ImageId);
        }

        if (Status.HasValue)
        {
            AddSearchAction(nameof(JobInfoModel.Status), Status);
        }
    }
}