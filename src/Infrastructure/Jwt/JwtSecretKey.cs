using System.Text;
using Microsoft.IdentityModel.Tokens;
using Utils.ValueObjects;

namespace WebHost.Infrastructure.Jwt
{
    public class JwtSecretKey : SymmetricSecurityKey
    {
        public JwtSecretKey(NonNullableString key)
            : this(key.Value())
        {
        }

        public JwtSecretKey(string key)
            : base(Encoding.UTF8.GetBytes(key))
        {
        }
    }
}