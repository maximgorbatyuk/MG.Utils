using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MG.Utils.EFCore
{
    public static class DatabaseConfigExtensions
    {
        public static DbContextOptionsBuilder IgnoreMultipleCollectionIncludeWarningWhen(this DbContextOptionsBuilder options, bool conditionToIgnore)
        {
            options.ConfigureWarnings(w =>
            {
                if (conditionToIgnore)
                {
                    // https://www.thinktecture.com/en/entity-framework-core/cartesian-explosion-problem-in-3-1/
                    // https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
                    w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning);
                }
            });

            return options;
        }
    }
}