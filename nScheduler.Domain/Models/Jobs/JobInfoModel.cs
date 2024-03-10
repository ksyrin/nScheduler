using Newtonsoft.Json;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using System.ComponentModel.DataAnnotations.Schema;

namespace nScheduler.Domain.Models.Jobs;

/// <summary>
/// 调度作业
/// </summary>
public class JobInfoModel : BaseAggregateRoot<Guid>
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 镜像Id
    /// </summary>
    public Guid ImageId { get; init; }

    /// <summary>
    /// 镜像
    /// </summary>
    public ImageCfg Image { get; set; }

    /// <summary>
    /// 相关参数配置
    /// </summary>
    public string ConfigsStr { get; private set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime ExecTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 时间间隔
    /// </summary>
    public string? Cron { get; init; }

    /// <summary>
    /// 依赖作业
    /// </summary>
    public string? PreJobId { get; init; }

    /// <summary>
    /// 当前状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 通知地址
    /// </summary>
    public Guid? MessageId { get; init; }

    [NotMapped]
    public string StatusString => Status switch
    {
        0 => "未运行",
        1 => "运行中",
        _ => "未知"
    };

    private JobInfoModel()
    { }

    public JobInfoModel(string id, string name, string imageId, IEnumerable<JobConfigModel> configs, DateTime startTime, DateTime execTime, DateTime endTime, string cron, string preJobId, string operName)
    {
        Id = id.IsEmpty() ? Guid.NewGuid() : id.ToGuid();
        Name = name;
        ImageId = imageId.ToGuid();
        SetJobConfigs(configs);
        StartTime = startTime;
        ExecTime = execTime;
        EndTime = endTime;
        Cron = cron;
        PreJobId = preJobId;
        Status = 0;
        OperName = operName;
        OperTime = DateTime.Now;
    }

    /// <summary>
    /// 获取参数
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IEnumerable<JobConfigModel> GetJobConfigs()
    {
        return ConfigsStr == null
            ? throw new ArgumentNullException("相关参数为空")
            : JsonConvert.DeserializeObject<IEnumerable<JobConfigModel>>(ConfigsStr) ?? throw new ArgumentNullException("相关参数查找失败");
    }

    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="configs"></param>
    public void SetJobConfigs(IEnumerable<JobConfigModel> configs)
    {
        ConfigsStr = JsonConvert.SerializeObject(configs);
    }
}

public class JobConfigModel
{
    public string ConfigName { get; set; }

    public ParameterType ConfigType { get; set; }

    public string ConfigValue { get; set; }
}