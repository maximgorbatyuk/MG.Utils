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
            int? port = null,
            bool checkForAuthentication = false,
            string roleToCheckForAuthorization = null,
            SqlEngine sqlEngine = default)
            where TDbContext : DbContext
        {
            return new DatabaseTablesSettings<TDbContext>(app, port, checkForAuthentication, roleToCheckForAuthorization, sqlEngine);
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
            return new MiddlewareRoute<DatabaseTablesMiddleware<TDbContext>, TDbContext>(
                settings: settings,
                path: path,
                methodName: HttpMethods.Get,
                defaultPathRoute: DefaultOutputRoute).Setup();
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
            return new MiddlewareRoute<ReadSQlMiddleware<TDbContext>, TDbContext>(
                settings: settings,
                path: path,
                methodName: HttpMethods.Post,
                defaultPathRoute: DefaultReadRoute).Setup();
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
            return new MiddlewareRoute<ExecuteSQlMiddleware<TDbContext>, TDbContext>(
                settings: settings,
                path: path,
                methodName: HttpMethods.Post,
                defaultPathRoute: DefaultExecuteRoute).Setup();
        }
    }
}