using System.Linq.Expressions;

namespace nScheduler.Common.Models;

public enum SearchType
{
    Like,
    Equal,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}

public class ISearchModel<T>
{
    protected List<(string, object, SearchType)> SearchExp = new();

    public Expression<Func<T, bool>>? GetExpression()
    {
        CreateSearchFunc();
        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression filter = null;
        foreach (var exp in SearchExp)
        {
            var tmp = CreateExpression(parameter, exp.Item1, exp.Item2, exp.Item3);
            filter = AddExpression(filter, tmp);
        }

        return filter != null ?
            Expression.Lambda<Func<T, bool>>(filter, parameter)
            : null;
    }

    public void AddSearchAction(string propertName, object propertValue, SearchType searchType = SearchType.Equal)
    {
        SearchExp.Add((propertName, propertValue, searchType));
    }

    public virtual void CreateSearchFunc()
    {
    }

    private static Expression? CreateExpression(ParameterExpression parameter,
        string propertName,
        object propertValue,
        SearchType ExpressionType)
    {
        Expression key = Expression.Property(parameter, propertName);
        Expression value = Expression.Constant(propertValue);
        return ExpressionType switch
        {
            SearchType.Like => Expression.Call(key, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value),
            SearchType.Equal => Expression.Equal(key, Expression.Convert(value, key.Type)),
            SearchType.GreaterThan => Expression.GreaterThan(key, Expression.Convert(value, key.Type)),
            SearchType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(key, Expression.Convert(value, key.Type)),
            SearchType.LessThan => Expression.LessThan(key, Expression.Convert(value, key.Type)),
            SearchType.LessThanOrEqual => Expression.LessThanOrEqual(key, Expression.Convert(value, key.Type)),
            _ => null
        };
    }

    private static Expression? AddExpression(Expression? main, Expression? last)
    {
        if (last == null)
            return main;

        if (main == null)
            return last;

        return Expression.AndAlso(main, last);
    }
}