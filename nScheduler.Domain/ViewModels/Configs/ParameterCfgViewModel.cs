using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Configs;

/// <summary>
/// 编辑视图模型
/// </summary>
public class ParameterCfgEditViewModel
{
    public string Name { get; set; }

    public string Content { get; set; }
}

/// <summary>
/// 列表视图模型
/// </summary>
public class ParameterCfgListViewModel
{
    public string Id { get; set; }

    [DisplayName("参数名称")]
    public string Name { get; set; }

    public string Content { get; set; }

    [DisplayName("操作人")]
    public string OperName { get; set; }

    [DisplayName("操作时间")]
    public string OperDate { get; set; }
}