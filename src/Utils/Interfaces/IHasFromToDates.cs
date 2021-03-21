using System;
using Utils.Exceptions;

namespace Utils.Interfaces
{
    public interface IHasFromToDates
    {
        DateTimeOffset From { get; set; }

        DateTimeOffset? To { get; set; }
    }
}