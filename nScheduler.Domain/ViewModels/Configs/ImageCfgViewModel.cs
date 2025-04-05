using nScheduler.Common.Models;
using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Configs;

public class ImageCfgViewModel
{
    public string Id { get; set; }

    [DisplayName("名称")]
    public string Name { get; set; }

    [DisplayName("镜像地址")]
    public string ImageName { get; set; }

    public List<ImageCfgParamViewModel> Configs { get; set; } = new();

    [DisplayName("操作人")]
    public string OperName { get; set; }

    [DisplayName("操作时间")]
    public string OperDate { get; set; }
}

public class ImageCfgParamViewModel
{
    [DisplayName("配置名称")]
    public string ParamName { get; set; }

    [DisplayName("配置类型")]
    public ParameterType ParamType { get; set; }
}

public class ImageCfgEditViewModel
{
    public string Name { get; set; }

    public string ImageName { get; set; }

    public Dictionary<string, ParameterType> ConfigParams { get; set; } = new();
}