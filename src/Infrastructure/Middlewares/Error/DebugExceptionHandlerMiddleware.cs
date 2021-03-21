using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace MG.WebHost.Infrastructure.Middlewares.Error
{
    public class DebugExceptionHandlerMiddleware : ExceptionHandlerMiddleware
    {
        public DebugExceptionHandlerMiddleware(RequestDelegate next, IStringLocalizer localizer)
            : base(next, localizer)
        {
        }

        protected override Task WriteResponseAsync(HttpContext context, int statusCode, string message, Exception exception)
        {
            return new JsonErrorResponse<DebugErrorDetails>(
                context: context,
                error: new DebugErrorDetails(statusCode, message, exception),
                statusCode: statusCode).WriteAsync();
        }

        private class DebugErrorDetails : ErrorDetails
        {
            public DebugErrorDetails(int status, string message, Exception exception)
                : base(status, message)
            {
                Exception = exception;
            }

            public Exception Exception { get; }
        }
    }
}