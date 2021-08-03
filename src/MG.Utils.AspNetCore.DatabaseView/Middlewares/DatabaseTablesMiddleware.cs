using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView.Middlewares
{
    public class DatabaseTablesMiddleware<TDbContext> : DatabaseTableBaseMiddleware<TDbContext>
        where TDbContext : DbContext
    {
        public DatabaseTablesMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        private async Task<string> PageAsync(TDbContext db, string tableName)
        {
            var table = await new ReadTableSqlCommand<TDbContext>(query: $"SELECT * FROM {tableName}", db).AsDataTableAsync();

            return new DataTableTextOutput(table).AsText();
        }

        private static string TableNameOrFail(HttpContext context)
        {
            string tableName = null;
            if (context.Request.Query.TryGetValue("tableName", out var value))
            {
                tableName = value.FirstOrDefault();
            }

            if (tableName is null)
            {
                throw new InvalidOperationException("You have to provide table name");
            }

            return tableName;
        }

        protected override Task<string> ResponseContentAsync(HttpContext httpContext, TDbContext context)
        {
            return PageAsync(context, TableNameOrFail(httpContext));
        }
    }
}