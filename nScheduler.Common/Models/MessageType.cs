using System.ComponentModel;

namespace nScheduler.Common.Models;

public enum MessageType
{
    [Description("钉钉")]
    DingTalk,

    [Description("企业微信")]
    Wechat
}