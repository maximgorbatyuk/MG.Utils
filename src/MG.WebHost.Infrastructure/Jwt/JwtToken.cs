using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.IdentityModel.Tokens;

namespace MG.WebHost.Infrastructure.Jwt
{
    // https://www.c-sharpcorner.com/article/how-to-use-jwt-authentication-with-web-api/
    public class JwtToken
    {
        private readonly JwtSecretKey _secret;
        private readonly ClaimsIdentity _user;
        private readonly int _expireAfterDays;

        private readonly Lazy<Contracts.Authentication.Jwt> _jwt;

        public JwtToken(NonNullableString secretKey, ClaimsIdentity user, int expireAfterDays = 1)
        {
            _user = user;
            _expireAfterDays = expireAfterDays;
            _secret = new JwtSecretKey(secretKey);

            _jwt = new Lazy<Contracts.Authentication.Jwt>(AsJwtInternal);
        }

        public Contracts.Authentication.Jwt Get() => _jwt.Value;

        private Contracts.Authentication.Jwt AsJwtInternal()
        {
            var handler = new JwtSecurityTokenHandler();

            var expiresAt = DateTime.Now.AddDays(_expireAfterDays);

            JwtSecurityToken token = handler.CreateJwtSecurityToken(
                issuer: "CT",
                audience: "users",
                subject: _user,
                expires: expiresAt,
                issuedAt: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    key: _secret,
                    algorithm: SecurityAlgorithms.HmacSha256Signature));

            string apiToken = handler.WriteToken(token);
            return new Contracts.Authentication.Jwt(apiToken, expiresAt);
        }

        public static implicit operator Contracts.Authentication.Jwt(JwtToken token)
        {
            return token!.Get();
        }
    }
}