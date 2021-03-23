using System.Text;
using MG.Utils.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace MG.WebHost.Infrastructure.Jwt
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