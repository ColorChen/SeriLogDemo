using Serilog;

namespace SeriLogDemo_Net6.Extensions
{
    public static class CustomMiddleware
    {
        public static IApplicationBuilder UseSerilogRequestResponseLogging(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestResponseLoggingMiddleware>();
            builder.UseSerilogRequestLogging(opts => {
                opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
            });
            return builder;
        }
    }
}
