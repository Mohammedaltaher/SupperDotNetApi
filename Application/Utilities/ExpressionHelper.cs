using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Utilities;

public static class ExpressionHelper
{

    public static void RegisterValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorType = typeof(IValidator<>);
        var types = assembly.GetExportedTypes()
            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == validatorType))
            .ToList();

        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces().First(y => y.IsGenericType && y.GetGenericTypeDefinition() == validatorType);
            services.AddScoped(interfaceType, type);
        }
    }

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