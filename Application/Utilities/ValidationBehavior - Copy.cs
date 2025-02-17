using System.Linq.Expressions;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> CombineWithAnd<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var combinedExpression = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            ),
            parameter
        );

        return combinedExpression;
    }
    public static Expression<Func<T, bool>> CombineWithAnd<T, TProperty>(
       this Expression<Func<T, bool>> first,
       Expression<Func<T, List<TProperty>>> propertySelector,
       Expression<Func<TProperty, bool>> condition)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var combinedExpression = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Call(
                    typeof(Enumerable),
                    "Any",
                    new[] { typeof(TProperty) },
                    Expression.Property(parameter, (propertySelector.Body as MemberExpression)?.Member as System.Reflection.PropertyInfo ?? throw new InvalidOperationException("Invalid propertySelector")),
                    condition
                )
            ),
            parameter
        );

        return combinedExpression;
    }
}