using Microsoft.EntityFrameworkCore;

namespace MG.Utils.AspNetCore.DatabaseView
{
    public interface IDatabaseTablesSettings<TContext> : IDatabaseTablesSettingsBase
        where TContext : DbContext
    {
    }
}