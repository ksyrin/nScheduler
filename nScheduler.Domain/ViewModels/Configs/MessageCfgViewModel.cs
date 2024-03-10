using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Configs;

/// <summary>
/// 消息视图模型
/// </summary>
public class MessageCfgViewModel
{
    public string Id { get; set; }

    [DisplayName("消息名称")]
    public string Name { get; set; }

    public int MsgType { get; set; }

    [DisplayName("消息类型")]
    public string MsgTypeStr => MsgType switch
    {
        0 => "钉钉",
        1 => "企业微信",
        _ => "未知"
    };

    [DisplayName("Url地址")]
    public string Url { get; set; }

    [DisplayName("操作人")]
    public string OperName { get; set; }

    [DisplayName("操作时间")]
    public string OperDate { get; set; }
}

/// <summary>
/// 消息编辑模型
/// </summary>
public class MessageCfgEditModel
{
    public string Name { get; set; }

    public int Type { get; set; }

    public string Url { get; set; }
}