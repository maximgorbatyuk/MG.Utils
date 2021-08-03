using MG.Utils.AspNetCore.DatabaseView.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    public static class DatabaseTablesViewExtensions
    {
        public const string DefaultOutputRoute = "/debug/database-tables/view";
        public const string DefaultReadRoute = "/debug/database-tables/read";
        public const string DefaultExecuteRoute = "/debug/database-tables/execute";

        public static IDatabaseTablesSettings<TDbContext> UseDatabaseTable<TDbContext>(
            this IApplicationBuilder app,
            int? port = null)
            where TDbContext : DbContext
        {
            return new DatabaseTablesSettings<TDbContext>(app, port);
        }

        /// <summary>
        /// GET /database-tables/view is a default route.
        /// </summary>
        /// <typeparam name="TDbContext">Database context.</typeparam>
        /// <param name="settings">Settings.</param>
        /// <param name="path">Path.</param>
        /// <returns>Settings instance.</returns>
        public static IDatabaseTablesSettings<TDbContext> UseTableOutputEndpoint<TDbContext>(this IDatabaseTablesSettings<TDbContext> settings, PathString path = default)
            where TDbContext : DbContext
        {
            if (!path.HasValue)
            {
                path = new PathString(DefaultOutputRoute);
            }

            // We allow you to listen on all URLs by providing the empty PathString.
            // If you do provide a PathString, want to handle all of the special cases that
            // StartsWithSegments handles, but we also want it to have exact match semantics.
            //
            // Ex: /Foo/ == /Foo (true)
            // Ex: /Foo/Bar == /Foo (false)
            settings.App.MapWhen(
                c => (settings.Port == null || c.Connection.LocalPort == settings.Port) &&
                     c.Request.Method == HttpMethods.Get &&
                             (!path.HasValue ||
                              (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                               string.IsNullOrEmpty(remaining))),
                b => b.UseMiddleware<DatabaseTablesMiddleware<TDbContext>>());
            return settings;
        }

        /// <summary>
        /// Executes and read any SQL command. POST /database-tables/read is a default route.
        /// </summary>
        /// <typeparam name="TDbContext">Database context.</typeparam>
        /// <param name="settings">Settings.</param>
        /// <param name="path">Path.</param>
        /// <returns>Settings instance.</returns>
        public static IDatabaseTablesSettings<TDbContext> UseReadEndpoint<TDbContext>(this IDatabaseTablesSettings<TDbContext> settings, PathString path = default)
            where TDbContext : DbContext
        {
            if (!path.HasValue)
            {
                path = new PathString(DefaultReadRoute);
            }

            settings.App.MapWhen(
                c => (settings.Port == null || c.Connection.LocalPort == settings.Port) &&
                     c.Request.Method == HttpMethods.Post &&
                     (!path.HasValue ||
                      (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                       string.IsNullOrEmpty(remaining))),
                b => b.UseMiddleware<ReadSQlMiddleware<TDbContext>>());
            return settings;
        }

        /// <summary>
        /// Executes any changing SQL command. POST /database-tables/execute is a default route.
        /// </summary>
        /// <typeparam name="TDbContext">Database context.</typeparam>
        /// <param name="settings">Settings.</param>
        /// <param name="path">Path.</param>
        /// <returns>Settings instance.</returns>
        public static IDatabaseTablesSettings<TDbContext> UseExecuteEndpoint<TDbContext>(this IDatabaseTablesSettings<TDbContext> settings, PathString path = default)
            where TDbContext : DbContext
        {
            if (!path.HasValue)
            {
                path = new PathString(DefaultExecuteRoute);
            }

            settings.App.MapWhen(
                c => (settings.Port == null || c.Connection.LocalPort == settings.Port) &&
                     c.Request.Method == HttpMethods.Post &&
                     (!path.HasValue ||
                      (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                       string.IsNullOrEmpty(remaining))),
                b => b.UseMiddleware<ExecuteSQlMiddleware<TDbContext>>());
            return settings;
        }
    }
}