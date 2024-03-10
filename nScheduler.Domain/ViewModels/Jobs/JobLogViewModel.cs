using nScheduler.Common.Models;
using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobLogViewModel
{
    [DisplayName("作业编码")]
    public string Id { get; set; }

    public string ImageId { get; set; }

    [DisplayName("作业类型")]
    public string ImageName { get; set; }

    [DisplayName("作业名称")]
    public string JobName { get; set; }

    [DisplayName("开始时间")]
    public string StartTime { get; set; }

    [DisplayName("结束时间")]
    public string EndTime { get; set; }

    public JobStatus State { get; set; }

    [DisplayName("状态")]
    public string StateStr => State switch
    {
        JobStatus.Running => "运行中",
        JobStatus.Completed => "结束",
        JobStatus.Error => "异常",
        _ => "未知"
    };

    public string Content { get; set; }
}