using nScheduler.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace nScheduler.Domain.Models.Jobs;

public class JobLogModel : IAggregateRoot<Guid>
{
    public Guid Id { get; init; }

    /// <summary>
    /// 镜像Id
    /// </summary>
    public Guid ImageId { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 作业状态
    /// </summary>
    public JobStatus JobState { get; set; }

    /// <summary>
    /// 开始执行时间
    /// </summary>
    public DateTime BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 运行时间
    /// </summary>
    public double IntervalTime { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 作业信息
    /// </summary>
    [NotMapped]
    public JobInfoModel Job { get; set; }

    private JobLogModel()
    { }

    public JobLogModel(Guid id, Guid imageId, string jobName, DateTime startTime, JobStatus jobState, JobInfoModel job)
    {
        Id = id;
        ImageId = imageId;
        JobName = jobName;
        BeginTime = startTime;
        JobState = jobState;
        Job = job;
    }

    public void UpdateStatus(JobStatus status)
    {
        JobState = status;
        if (status is JobStatus.Completed or JobStatus.Error)
        {
            EndTime = DateTime.Now;
            IntervalTime = Math.Round((EndTime.Value - BeginTime).TotalSeconds, 0);
        }
    }

    public bool IsFinish => JobState is JobStatus.Completed or JobStatus.Error;
}