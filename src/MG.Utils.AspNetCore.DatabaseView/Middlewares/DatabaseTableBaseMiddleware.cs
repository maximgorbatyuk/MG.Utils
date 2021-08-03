using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView.Middlewares
{
    public abstract class DatabaseTableBaseMiddleware<TDbContext>
        where TDbContext : DbContext
    {
        private readonly RequestDelegate _next;
        private readonly string _contentType;

        protected DatabaseTableBaseMiddleware(RequestDelegate next, string contentType = "text/html; charset=UTF-8")
        {
            _next = next;
            _contentType = contentType;
        }

        public async Task InvokeAsync(HttpContext context, TDbContext db)
        {
            context.Response.ContentType = _contentType;
            await context.Response.WriteAsync(await ResponseContentAsync(context, db));
        }

        protected abstract Task<string> ResponseContentAsync(HttpContext httpContext, TDbContext context);
    }
}