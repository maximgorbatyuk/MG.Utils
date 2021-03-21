using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace WebHost.Infrastructure.Attributes
{
    public class AzureAuthorizeAttribute : AuthorizeAttribute
    {
        public AzureAuthorizeAttribute()
        {
            AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme;
        }
    }
}