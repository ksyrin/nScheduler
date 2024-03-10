using System.ComponentModel;

namespace nScheduler.Common.Models;

public enum ParameterType
{
    [Description("自定义")]
    input,

    [Description("配置参数")]
    param
}