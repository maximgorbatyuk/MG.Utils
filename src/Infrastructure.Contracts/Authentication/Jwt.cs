﻿using System;

namespace WebHost.Infrastructure.Contracts.Authentication
{
    public readonly struct Jwt
    {
        public Jwt(string apiToken, DateTimeOffset expiresAt)
        {
            ApiToken = apiToken;
            ExpiresAt = expiresAt;
        }

        public string ApiToken { get; }

        public DateTimeOffset ExpiresAt { get; }

        public string TokenType => "Bearer";

        public override string ToString() => ApiToken;
    }
}