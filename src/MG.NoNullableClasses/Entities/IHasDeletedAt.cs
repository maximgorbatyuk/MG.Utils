using System;

namespace MG.Utils.Interfaces
{
    public interface IHasDeletedAt
    {
        DateTimeOffset? DeletedAt { get; }
    }
}