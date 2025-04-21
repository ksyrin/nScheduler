using nScheduler.Common.Models;
using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobLogDetailViewModel
{
    [DisplayName("开始时间")]
    public string StartTime { get; set; }

    [DisplayName("结束时间")]
    public string EndTime { get; set; }

    [DisplayName("执行时间")]
    public double IntervalTime { get; set; }

    public JobStatus State { get; set; }

    [DisplayName("状态")]
    public string StateStr => State switch
    {
        JobStatus.Running => "运行中",
        JobStatus.Completed => "结束",
        JobStatus.Error => "异常",
        _ => "未知"
    };
}