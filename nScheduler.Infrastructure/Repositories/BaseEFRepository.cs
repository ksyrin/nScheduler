using System;
using System.Linq.Expressions;
using nScheduler.Domain.Entities;
using nScheduler.Domain.Repositories;

namespace nScheduler.Infrastructure.Repositories;

public abstract class BaseEFRepository<T> : IBaseRepository<T>
    where T : class, IAggregateRoot
{
    private readonly DataContext context;

    public BaseEFRepository(DataContext context)
    {
        this.context = context;
    }

    public bool Delete(Guid Id)
    {
        var entity = Find(Id);
        if (entity != null)
            context.Remove(entity);
        return context.SaveChanges() > 0;
    }

    public bool Edit(T config)
    {
        var entity = Find(config.Id);
        if (entity == null)
        {
            context.Set<T>().Add(config);
        }
        else
        {
            context.Entry(entity).CurrentValues.SetValues(config);
        }
        return context.SaveChanges() > 0;
    }

    public T? Find(Guid Id)
    {
        return context.Set<T>().Find(Id);
    }

    public IEnumerable<T> List(Func<T, bool> expression, int page, int size)
    {
        return context.Set<T>().Where(expression).Skip((page - 1) * size).Take(size);
    }
}

