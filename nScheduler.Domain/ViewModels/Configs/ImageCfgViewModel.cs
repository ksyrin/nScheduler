using nScheduler.Common.Models;
using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Configs;

public class ImageCfgParamModel
{
    public string ParamName { get; set; }

    public ParameterType ParamType { get; set; }
}

public class ImageCfgViewModel
{
    public string Id { get; set; }

    [DisplayName("名称")]
    public string Name { get; set; }

    [DisplayName("镜像地址")]
    public string ImageName { get; set; }

    public List<ImageCfgParamModel> Configs { get; set; } = new();

    [DisplayName("操作人")]
    public string OperName { get; set; }

    [DisplayName("操作时间")]
    public string OperDate { get; set; }
}

public class ImageCfgEditModel
{
    public string Name { get; set; }

    public string ImageName { get; set; }

    public Dictionary<string, ParameterType> ConfigParams { get; set; } = new();
}