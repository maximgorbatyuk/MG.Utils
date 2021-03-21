﻿using MG.Utils.Helpers;

namespace MG.WebHost.Infrastructure.Middlewares.Error
{
    public record ValidationError
    {
        public ValidationError(string name, string description)
        {
            Name = name.ThrowIfNullOrEmpty(nameof(name));
            Description = description.ThrowIfNullOrEmpty(nameof(description));
        }

        public string Name { get; }

        public string Description { get; }
    }
}