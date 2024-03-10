using Newtonsoft.Json;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;

namespace nScheduler.Domain.Models.Configs;

/// <summary>
/// 配置容器所需的镜像
/// </summary>
public class ImageCfg : BaseAggregateRoot<Guid>
{
    private ImageCfg()
    {
    }

    public ImageCfg(string id, string name, string imageName, Dictionary<string, ParameterType> configs, string operName)
    {
        Id = id.IsEmpty() ? Guid.NewGuid() : id.ToGuid();
        Name = name;
        ImageName = imageName;
        ConfigString = JsonConvert.SerializeObject(configs);
        OperName = operName;
        OperTime = DateTime.Now;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 镜像地址
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    /// 镜像参数
    /// </summary>
    public string ConfigString { get; set; }

    public Dictionary<string, ParameterType> GetConfigs()
    {
        if (string.IsNullOrWhiteSpace(ConfigString)) throw new ArgumentNullException("找不到镜像参数");

        return JsonConvert.DeserializeObject<Dictionary<string, ParameterType>>(ConfigString) ?? throw new InvalidOperationException("镜像参数格式有问题");
    }
}