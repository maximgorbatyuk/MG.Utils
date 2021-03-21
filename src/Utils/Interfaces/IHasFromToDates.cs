using System;

namespace MG.Utils.Interfaces
{
    public interface IHasFromToDates
    {
        DateTimeOffset From { get; set; }

        DateTimeOffset? To { get; set; }
    }
}