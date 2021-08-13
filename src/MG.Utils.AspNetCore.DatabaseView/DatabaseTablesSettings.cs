using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class DatabaseTablesSettings<TContext> : IDatabaseTablesSettings<TContext>
        where TContext : DbContext
    {
        public IApplicationBuilder App { get; }

        public int? Port { get; }

        public bool CheckForAuthentication { get; }

        public string RoleToCheckForAuthorization { get; }

        public SqlEngine SqlEngine { get; }

        public DatabaseTablesSettings(IApplicationBuilder app, int? port, bool checkForAuthentication, string roleToCheckForAuthorization, SqlEngine sqlEngine)
        {
            App = app;
            Port = port;
            CheckForAuthentication = checkForAuthentication;
            RoleToCheckForAuthorization = roleToCheckForAuthorization;
            SqlEngine = sqlEngine;
        }
    }
}