using System;

namespace Utils.Interfaces
{
    public interface IHasDeletedAt
    {
        DateTimeOffset? DeletedAt { get; }
    }
}