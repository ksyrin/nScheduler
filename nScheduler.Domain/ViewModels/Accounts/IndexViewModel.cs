namespace nScheduler.Domain.ViewModels.Accounts;

public class IndexViewModel
{
    public int JobAllCount { get; set; }

    public int JobCanRunCount { get; set; }

    public int ImageCount { get; set; }

    public int ParamCount { get; set; }

    public int MessageCount { get; set; }

    public int UserCount { get; set; }

    /// <summary>
    /// 近30天作业数量
    /// </summary>
    public Dictionary<string, int> JobLogGroupDay { get; set; } = new();

    /// <summary>
    /// KKS分类分组统计
    /// </summary>
    public Dictionary<string, int> JobTypeGroupDay { get; init; } = new();
}

/// <summary>
/// TestKKSGroupData
/// </summary>
public class TestKKSGroupData
{
    /// <summary>
    /// KKS
    /// </summary>
    public string KKS { get; init; } = string.Empty;

    /// <summary>
    /// NAM
    /// </summary>
    public string NAM { get; init; } = string.Empty;

    /// <summary>
    /// Count
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// ApprovedCount
    /// </summary>
    public int ApprovedCount { get; set; }

    /// <summary>
    /// Percent
    /// </summary>
    public double Percent { get; set; }
}