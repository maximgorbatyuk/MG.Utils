using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MG.Utils.Azure.Authentication.Attributes
{
    public class BearerAuthorizeAttribute : AuthorizeAttribute
    {
        public BearerAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}