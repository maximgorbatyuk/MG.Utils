using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView.Middlewares
{
    public class ExecuteSQlMiddleware<TDbContext> : BasePostRequestMiddleware<TDbContext>
        where TDbContext : DbContext
    {
        public ExecuteSQlMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        protected override async Task<string> ResponseContentAsync(string query, HttpContext httpContext, TDbContext context)
        {
            var result = await context.Database.ExecuteSqlRawAsync(query);
            return $"Rows affected: {result}";
        }
    }
}