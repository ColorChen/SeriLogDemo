using Autofac.Core;
using Dapper.Extensions;
using Dapper.Extensions.Caching.Redis;
using Dapper.Extensions.MSSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using SeriLogDemo_Net6;
using SeriLogDemo_Net6.Extensions;
using SeriLogDemo_Net6.Repository;
using StackExchange.Redis;
using System.Security.Cryptography.Xml;

var win = Environment.GetEnvironmentVariable("windir");
Console.WriteLine(win);


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .CreateLogger();

var env = builder.Environment;
var configBuilder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json")
                     .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

var config = configBuilder.Build();
var seriLogDemoConfig = new SeriLogDemoConfig();
config.Bind(seriLogDemoConfig);

Console.WriteLine($"EnvironmentName :{seriLogDemoConfig.EnvironmentName}");

// Add services to the container.
try
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDapperForMSSQL();


    builder.Services.AddDapperCachingInRedis(new RedisConfiguration
    {
        AllMethodsEnableCache = false,
        KeyPrefix = "RedisNet6",
        ConnectionString = SeriLogDemoConfig.ConnectionStrings.RedisConnection
    });




    builder.Services.AddScoped<WorkRepository>(o=>new WorkRepository(SeriLogDemoConfig.ConnectionStrings.DefaultConnection,o.GetRequiredService<IDiagnosticContext>()));

    builder.Services.AddMemoryCache();

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions() {
        EndPoints={ $"{SeriLogDemoConfig.ConnectionStrings.RedisConnection}" },
        AbortOnConnectFail = false
    }));
                 
    builder.Host.UseSerilog();

    

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSerilogRequestResponseLogging();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    app.UseEndpoints(endpoint =>
    {
        endpoint.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Process name:" + System.Diagnostics.Process.GetCurrentProcess().ProcessName);
        });
    });




    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}




