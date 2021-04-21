using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MG.Utils.AspNetCore.DatabaseView.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    public class DatabaseTablesMiddleware<TDbContext>
        where TDbContext : DbContext
    {
        private readonly RequestDelegate _next;

        public DatabaseTablesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TDbContext db)
        {
            context.Response.ContentType = "text/html; charset=UTF-8";
            await context.Response.WriteAsync(await PageAsync(db, TableNameOrNull(context)));
        }

        private async Task<string> PageAsync(TDbContext db, string tableNameOrNull)
        {
            IReadOnlyCollection<TableView> tables = await new DatabaseView<TDbContext>(db, tableNameOrNull).TablesAsync();

            return new Html(tables);
        }

        private string TableNameOrNull(HttpContext context)
        {
            return context.Request.Query.TryGetValue("tableName", out var value) ? value.FirstOrDefault() : null;
        }
    }
}