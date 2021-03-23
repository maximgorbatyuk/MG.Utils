using MG.WebHost.Infrastructure.Middlewares.Error;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MG.WebHost.Infrastructure.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return env.IsDevelopment() || env.IsEnvironment("Demo")
                ? app.UseMiddleware<DebugExceptionHandlerMiddleware>()
                : app.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}