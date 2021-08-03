using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView.Middlewares
{
    public abstract class BasePostRequestMiddleware<TDbContext> : DatabaseTableBaseMiddleware<TDbContext>
        where TDbContext : DbContext
    {
        protected BasePostRequestMiddleware(RequestDelegate next, string contentType = "text/html; charset=UTF-8")
            : base(next, contentType)
        {
        }

        protected override async Task<string> ResponseContentAsync(HttpContext httpContext, TDbContext context)
        {
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(httpContext.Request.Body);
            var body = await reader.ReadToEndAsync();

            var bodyParams = body.Split('&');

            if (bodyParams.Length != 1 || (bodyParams.Length > 1 && !bodyParams[0].StartsWith("query=")))
            {
                throw new InvalidOperationException($"The request body doesn't contain query. {body}");
            }

            var query = bodyParams[0].Replace("query=", string.Empty);

            return await ResponseContentAsync(query, httpContext, context);
        }

        protected abstract Task<string> ResponseContentAsync(string query, HttpContext httpContext, TDbContext context);
    }
}