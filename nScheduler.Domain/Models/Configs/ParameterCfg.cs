using nScheduler.Common.Extensions;

namespace nScheduler.Domain.Models.Configs;

/// <summary>
/// 参数变量配置
/// </summary>
public class ParameterCfg : BaseAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 加密内容
    /// </summary>
    public string Content { get; init; }

    private ParameterCfg()
    { }

    public ParameterCfg(string id, string name, string content, string operName)
    {
        Id = id.IsEmpty() ? Guid.NewGuid() : id.ToGuid();
        Name = name;
        Content = content;
        OperName = operName;
        OperTime = DateTime.Now;
    }
}