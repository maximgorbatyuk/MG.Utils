using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    public static class DatabaseTablesViewExtensions
    {
        public const string DefaultRoute = "/database-tables/view";

        /// <summary>
        /// /database-tables/view is a default route.
        /// </summary>
        /// <typeparam name="TDbContext">Database context.</typeparam>
        /// <param name="app">App.</param>
        /// <param name="path">Path.</param>
        /// <param name="port">Port.</param>
        public static void UseDatabaseTableHtml<TDbContext>(this IApplicationBuilder app, PathString path = default(PathString), int? port = null)
            where TDbContext : DbContext
        {
            if (!path.HasValue)
            {
                path = new PathString(DefaultRoute);
            }

            // We allow you to listen on all URLs by providing the empty PathString.
            // If you do provide a PathString, want to handle all of the special cases that
            // StartsWithSegments handles, but we also want it to have exact match semantics.
            //
            // Ex: /Foo/ == /Foo (true)
            // Ex: /Foo/Bar == /Foo (false)
            app.MapWhen(
                c => (port == null || c.Connection.LocalPort == port) &&
                             (!path.HasValue ||
                              (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                               string.IsNullOrEmpty(remaining))),
                b => b.UseMiddleware<DatabaseTablesMiddleware<TDbContext>>());
        }
    }
}