
using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using MediatR;
using Repository.Context;
using Implementation.UnitOfWorks;
using Repository.GenericRepository;
using Application.Utilities;

namespace Implementation.Extensions;

public static class UMCoreExtensions
{
    public static void AddCoreServies(this IServiceCollection services, IConfiguration configuration)
    {
        var isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        var DbConnection =  configuration.GetConnectionString(!isRunningInDocker? "Default" : "Docker")!;
        services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(DbConnection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        services.AddScoped<AppDbContext>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IUnitOfWork, UnitOfWork>();
   
        //services.AddHealthChecks().AddSqlServer(DbConnection).AddDbContextCheck<AppDbContext>();
        //services.AddScoped(typeof(ExceptionHandlingMiddleware<>));
        //services.AddScoped<ILocalizer, Localizer>();
        //services.AddScoped<IHttpClientServices, HttpClientServices>();
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

        RegisterValidators(services, Assembly.GetAssembly(typeof(Application.Features.FeatureModule))!);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void RegisterValidators(IServiceCollection services, Assembly assembly)
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
}
