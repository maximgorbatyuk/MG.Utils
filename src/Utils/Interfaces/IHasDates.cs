using System;

namespace MG.Utils.Interfaces
{
    public interface IHasDates
    {
        DateTimeOffset CreatedAt { get; }

        DateTimeOffset UpdatedAt { get; }

        void OnCreate(DateTimeOffset now);

        void OnUpdate(DateTimeOffset now);
    }
}