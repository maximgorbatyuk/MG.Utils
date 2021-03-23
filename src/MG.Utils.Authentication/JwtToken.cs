using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.IdentityModel.Tokens;

namespace MG.Utils.Authentication
{
    // https://www.c-sharpcorner.com/article/how-to-use-jwt-authentication-with-web-api/
    public class JwtToken
    {
        private readonly JwtSecretKey _secret;
        private readonly ClaimsIdentity _user;
        private readonly int _expireAfterDays;
        private readonly NonNullableString _issuer;
        private readonly NonNullableString _audience;

        private readonly Lazy<Jwt> _jwt;

        public JwtToken(
            NonNullableString secretKey,
            ClaimsIdentity user,
            NonNullableString issuer,
            NonNullableString audience,
            int expireAfterDays = 1)
        {
            _user = user;
            _issuer = issuer;
            _audience = audience;
            _expireAfterDays = expireAfterDays;
            _secret = new JwtSecretKey(secretKey);

            _jwt = new Lazy<Jwt>(AsJwtInternal);
        }

        public Jwt Get() => _jwt.Value;

        private Jwt AsJwtInternal()
        {
            var handler = new JwtSecurityTokenHandler();

            var expiresAt = DateTime.Now.AddDays(_expireAfterDays);

            JwtSecurityToken token = handler.CreateJwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                subject: _user,
                expires: expiresAt,
                issuedAt: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    key: _secret,
                    algorithm: SecurityAlgorithms.HmacSha256Signature));

            string apiToken = handler.WriteToken(token);
            return new Jwt(apiToken, expiresAt);
        }

        public static implicit operator Jwt(JwtToken token)
        {
            return token!.Get();
        }
    }
}