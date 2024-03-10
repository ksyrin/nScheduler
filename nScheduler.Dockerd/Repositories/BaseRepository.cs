using nScheduler.Domain.Models;
using nScheduler.Domain.Repositories;

namespace nScheduler.Dockerd.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : class, IAggregateRoot
{
    protected readonly DataContext context;

    public BaseRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<bool> Add(T model)
    {
        var entity = await Find(model.Id);
        if (entity == null)
        {
            context.Set<T>().Add(model);
        }
        else
        {
            throw new InvalidOperationException("数据已存在");
        }
        return (await context.SaveChangesAsync()) > 0;
    }

    public virtual Task<int> Count(Func<T, bool> expression)
    {
        return Task.FromResult(context.Set<T>()
            .Where(expression == null ? x => true : expression)
            .Count());
    }

    public virtual async Task<bool> Delete(Guid Id)
    {
        var entity = await Find(Id);
        if (entity != null)
            context.Remove(entity);
        return (await context.SaveChangesAsync()) > 0;
    }

    public virtual async Task<bool> Edit(T config)
    {
        var entity = await Find(config.Id);
        if (entity == null)
        {
            context.Set<T>().Add(config);
        }
        else
        {
            context.Entry(entity).CurrentValues.SetValues(config);
        }
        return (await context.SaveChangesAsync()) > 0;
    }

    public virtual ValueTask<T?> Find(Guid Id)
    {
        return context.Set<T>().FindAsync(Id);
    }

    public virtual Task<IEnumerable<T>> List(Func<T, bool> expression, int page, int size)
    {
        return Task.FromResult(context.Set<T>()
            .Where(expression == null ? x => true : expression)
            .Skip((page - 1) * size).Take(size));
    }

    public virtual Task<IEnumerable<T>> List(Func<T, bool> expression)
    {
        return Task.FromResult(context.Set<T>()
            .Where(expression == null ? x => true : expression));
    }

    public virtual async Task<(int, IEnumerable<T>)> Page(Func<T, bool> expression, int page, int size)
    {
        return (await Count(expression), await List(expression, page, size));
    }

    public async Task<bool> Update(T model)
    {
        var entity = await Find(model.Id);
        context.Entry(entity).CurrentValues.SetValues(model);

        return (await context.SaveChangesAsync()) > 0;
    }
}