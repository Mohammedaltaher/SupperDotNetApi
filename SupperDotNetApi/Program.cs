using Repository.Context;
using Serilog;
using Application.Features;
using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Repository.GenericRepository;
using Implementation.UnitOfWorks;
using Application.Utilities;
using System.Reflection;
using MediatR;
using SupperDotNetApi.Middleware;
using Core.Helper.Implementations;


var builder = WebApplication.CreateBuilder(args);
var isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var DbConnection = builder.Configuration.GetConnectionString(!isRunningInDocker ? "Default" : "Docker")!;


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(DbConnection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Add Redis Cache
var redisConfiguration =
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis").GetValue<string>("Configuration");
    options.InstanceName = "MyAppRedisInstance";
});
builder.Services.AddSingleton<IRedisCache, RedisCacheService>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

builder.Services.RegisterValidators(Assembly.GetAssembly(typeof(FeatureModule))!);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorMiddleware<,>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(FeatureModule).Assembly));

builder.Services.AddAutoMapper(typeof(FeatureModule).Assembly);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


