using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Models;
using nScheduler.Domain.Repositories;
using System.Linq.Expressions;

namespace nScheduler.Imp.Repositories;

public class BaseRepository<T, TItem> : IBaseRepository<T, TItem>
    where T : class, IAggregateRoot<TItem>
{
    protected readonly DataContext context;

    public BaseRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<bool> Add(T model, CancellationToken cancellationToken = default)
    {
        var entity = await Find(model.Id);
        if (entity == null)
        {
            await context.Set<T>().AddAsync(model, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("数据已存在");
        }
        return (await context.SaveChangesAsync(cancellationToken)) > 0;
    }

    public virtual async Task<bool> Delete(TItem Id, CancellationToken cancellationToken = default)
    {
        var entity = await Find(Id, cancellationToken);
        if (entity != null)
            context.Remove(entity);
        return (await context.SaveChangesAsync(cancellationToken)) > 0;
    }

    public virtual async Task<bool> Edit(T config, CancellationToken cancellationToken = default)
    {
        var entity = await Find(config.Id, cancellationToken);
        if (entity == null)
        {
            await context.Set<T>().AddAsync(config, cancellationToken);
        }
        else
        {
            context.Entry(entity).CurrentValues.SetValues(config);
        }
        return (await context.SaveChangesAsync(cancellationToken)) > 0;
    }

    public virtual ValueTask<T?> Find(TItem Id, CancellationToken cancellationToken = default)
    {
        return context.Set<T>().FindAsync(Id, cancellationToken);
    }

    public virtual Task<int> Count(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default)
    {
        return context.Set<T>()
            .CountAsync(expression ?? (x => true), cancellationToken);
    }

    public virtual Task<List<T>> List(Expression<Func<T, bool>>? expression, int page, int size, CancellationToken cancellationToken = default)
    {
        return context.Set<T>()
            .Where(expression ?? (x => true))
            .Skip((page - 1) * size).Take(size)
            .ToListAsync(cancellationToken);
    }

    public virtual Task<List<T>> List(Expression<Func<T, bool>>? expression, CancellationToken cancellationToken = default)
    {
        return context.Set<T>()
            .Where(expression ?? (x => true))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<(int, List<T>)> Page(Expression<Func<T, bool>>? expression, int page, int size, CancellationToken cancellationToken = default)
    {
        return (await Count(expression, cancellationToken), await List(expression, page, size, cancellationToken));
    }

    public async Task<bool> Update(T model, CancellationToken cancellationToken = default)
    {
        var entity = await Find(model.Id, cancellationToken);
        if (entity == null) return false;

        context.Entry(entity).CurrentValues.SetValues(model);
        return (await context.SaveChangesAsync(cancellationToken)) > 0;
    }
}