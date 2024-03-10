using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobListViewModel
{
    public string Id { get; set; }

    [DisplayName("作业名称")]
    public string Name { get; set; }

    [DisplayName("镜像名称")]
    public string ImageName { get; set; }

    [DisplayName("开始时间")]
    public string StartTime { get; set; }

    [DisplayName("执行时间")]
    public string ExecTime { get; set; }

    [DisplayName("结束时间")]
    public string EndTime { get; set; }

    public int State { get; set; }

    [DisplayName("状态")]
    public string StateStr => State switch
    {
        -1 => "失效",
        0 => "正常",
        1 => "运行中",
        _ => "未知"
    };
}