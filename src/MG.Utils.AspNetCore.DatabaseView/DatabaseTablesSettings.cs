using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class DatabaseTablesSettings<TContext> : IDatabaseTablesSettings<TContext>
        where TContext : DbContext
    {
        public IApplicationBuilder App { get; }

        public int? Port { get; }

        public DatabaseTablesSettings(IApplicationBuilder app, int? port)
        {
            App = app;
            Port = port;
        }
    }
}