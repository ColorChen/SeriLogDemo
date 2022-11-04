using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using SeriLogDemo_Net6;
using SeriLogDemo_Net6.Repository;

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

// Add services to the container.
try
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<WorkRepository>();

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    #region UseSerilogRequestLogging 參考範例
    //app.UseSerilogRequestLogging(options =>
    //{
    //    // 如果要自訂訊息的範本格式，可以修改這裡，但修改後並不會影響結構化記錄的屬性
    //    // options.MessageTemplate = "Handled {RequestPath}";

    //    // 預設輸出的紀錄等級為 Information，你可以在此修改記錄等級
    //    // options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

    //    // 你可以從 httpContext 取得 HttpContext 下所有可以取得的資訊！
    //    options.EnrichDiagnosticContext = async (diagnosticContext, httpContext) =>
    //    {
    //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
    //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);

    //        #region 自定義的屬性


    //        diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString.Value);
    //        diagnosticContext.Set("Protocol", httpContext.Request.Protocol);
    //        diagnosticContext.Set("RequestBody", await LogHelper.ReadBodyFromRequest(httpContext.Request));

    //        // Set the content-type of the Response at this point
    //        diagnosticContext.Set("ContentType", httpContext.Response.ContentType);
    //        #endregion

    //        diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
    //    };
    //});
    #endregion
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseSerilogRequestLogging(opts => {
        opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
    });


    app.UseAuthorization();

    app.MapControllers();

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




