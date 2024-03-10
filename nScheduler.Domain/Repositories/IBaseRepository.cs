using System.Linq.Expressions;

namespace nScheduler.Domain.Repositories;

public interface IBaseRepository<T, TItem>
{
    /// <summary>
    /// 查找指定Id
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    ValueTask<T?> Find(TItem Id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查找列表
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    Task<List<T>> List(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查找列表
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    Task<List<T>> List(Expression<Func<T, bool>>? expression, int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查找总数量
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<int> Count(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    Task<(int, List<T>)> Page(Expression<Func<T, bool>>? expression, int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Add(T model, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Update(T model, CancellationToken cancellationToken = default);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    Task<bool> Edit(T config, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<bool> Delete(TItem Id, CancellationToken cancellationToken = default);
}