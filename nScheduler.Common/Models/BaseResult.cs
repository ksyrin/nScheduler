namespace nScheduler.Common.Models;

public class BaseResult
{
    public int Code { get; set; } = 200;

    public string Msg { get; set; }
}

/// <summary>
/// 查询返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueryResult<T>
{
    public int Total { get; set; }

    public IEnumerable<T> Items { get; set; }
}