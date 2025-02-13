using Repository.Context;
using Serilog;
using Implementation.Extensions;
using Application.Features;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCoreServies(builder.Configuration);
builder.Services.AddMediatR(
     cfg =>
     {
         cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(FeatureModule).Assembly);
     });
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(FeatureModule).Assembly));
builder.Services.AddAutoMapper(typeof(FeatureModule).Assembly);
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
