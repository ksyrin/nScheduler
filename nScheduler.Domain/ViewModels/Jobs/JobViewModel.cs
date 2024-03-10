using nScheduler.Common.Models;

namespace nScheduler.Domain.ViewModels.Jobs;

public class JobConfigViewModel
{
    public string ConfigName { get; set; }

    public ParameterType ConfigType { get; set; }

    public string ConfigValue { get; set; }
}

public class JobViewModel
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 镜像名称
    /// </summary>
    public string ImageId { get; set; }

    /// <summary>
    /// 相关参数配置
    /// </summary>
    public List<JobConfigViewModel> Configs { get; set; } = new();

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 时间间隔
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 依赖作业
    /// </summary>
    public string PreJobId { get; set; }
}