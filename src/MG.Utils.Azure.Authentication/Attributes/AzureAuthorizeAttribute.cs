using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace MG.Utils.Azure.Authentication.Attributes
{
    public class AzureAuthorizeAttribute : AuthorizeAttribute
    {
        public AzureAuthorizeAttribute()
        {
            AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme;
        }
    }
}