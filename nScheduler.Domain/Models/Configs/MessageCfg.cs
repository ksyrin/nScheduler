using nScheduler.Common.Extensions;
using nScheduler.Common.Models;

namespace nScheduler.Domain.Models.Configs;

/// <summary>
/// 消息配置，用于通知
/// </summary>
public class MessageCfg : BaseAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public MessageType Type { get; init; }

    /// <summary>
    /// Url地址
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }

    private MessageCfg()
    { }

    public MessageCfg(string id, string name, string url, int type, string operName)
    {
        Id = id.IsEmpty() ? Guid.NewGuid() : id.ToGuid();
        Name = name;
        Url = url;
        Type = Enum.Parse<MessageType>(type.ToString());
        OperName = operName;
        OperTime = DateTime.Now;
    }
}