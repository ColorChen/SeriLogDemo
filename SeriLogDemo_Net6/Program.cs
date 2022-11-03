using Serilog;
using Serilog.Events;
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

    app.UseSerilogRequestLogging(options =>
    {
        // �p�G�n�ۭq�T�����d���榡�A�i�H�ק�o�̡A���ק��ä��|�v�T���c�ưO�����ݩ�
        // options.MessageTemplate = "Handled {RequestPath}";

        // �w�]��X���������Ŭ� Information�A�A�i�H�b���ק�O������
        // options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

        // �A�i�H�q httpContext ���o HttpContext �U�Ҧ��i�H���o����T�I
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
        };
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


