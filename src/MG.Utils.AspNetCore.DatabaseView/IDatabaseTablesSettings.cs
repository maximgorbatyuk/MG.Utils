using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    public interface IDatabaseTablesSettings<TContext>
        where TContext : DbContext
    {
        IApplicationBuilder App { get; }

        int? Port { get; }
    }
}