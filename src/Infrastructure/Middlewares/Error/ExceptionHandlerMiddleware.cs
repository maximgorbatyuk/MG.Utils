using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Utils.Exceptions;

namespace WebHost.Infrastructure.Middlewares.Error
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer _localizer;

        private readonly Dictionary<Type, int> _statusCodeConversion = new ()
        {
            { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
            { typeof(AuthenticationException), StatusCodes.Status401Unauthorized },
            { typeof(NoPermissionsException), StatusCodes.Status403Forbidden },
            { typeof(ResourceNotFoundException), StatusCodes.Status404NotFound },
            { typeof(BadRequestException), StatusCodes.Status400BadRequest },
            { typeof(EntityInvalidException), StatusCodes.Status400BadRequest },
            { typeof(InputValidationException), StatusCodes.Status422UnprocessableEntity },
            { typeof(InvalidOperationException), StatusCodes.Status400BadRequest },
            { typeof(DbUpdateConcurrencyException), StatusCodes.Status409Conflict }
        };

        public ExceptionHandlerMiddleware(RequestDelegate next, IStringLocalizer localizer)
        {
            _next = next;
            _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            string message = null;

            if (exception.GetType() == typeof(NotImplementedException))
            {
                message = "The endpoint is not implemented yet. Please, keep calm and wait for a while";
            }
            else if (_statusCodeConversion.TryGetValue(exception.GetType(), out int status))
            {
                statusCode = status;
                message = _localizer[exception.Message];
            }

            await WriteResponseAsync(context, statusCode, message, exception);
        }

        protected virtual Task WriteResponseAsync(
            HttpContext context, int statusCode, string message, Exception exception)
        {
            return new JsonErrorResponse<ErrorDetails>(
                context: context,
                error: new ErrorDetails(statusCode, message),
                statusCode: statusCode).WriteAsync();
        }
    }
}