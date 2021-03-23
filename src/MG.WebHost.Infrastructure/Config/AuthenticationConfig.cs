using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

namespace MG.WebHost.Infrastructure.Config
{
    public abstract class AuthenticationConfig
    {
        private readonly IServiceCollection _services;

        public AuthenticationConfig(IServiceCollection services)
        {
            _services = services;
        }

        // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/
        public AuthenticationConfig AzureActiveDirectorySso()
        {
            // https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp
            _services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(AzureAdConfigSection())
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddDownstreamWebApi("DownstreamApi", x =>
                {
                    x.Scopes = "User.Read";
                })
                .AddDistributedTokenCaches();

            return this;
        }

        public AuthenticationConfig SelfBearer()
        {
            // https://metanit.com/sharp/aspnet5/23.7.php
            // https://jasonwatmore.com/post/2019/10/14/aspnet-core-3-simple-api-for-authentication-registration-and-user-management
            // https://github.com/dotnet/aspnetcore/issues/4632#issuecomment-444967075
            _services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new JwtSecretKey(JwtSecretKey()),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return this;
        }

        protected abstract IConfigurationSection AzureAdConfigSection();

        protected abstract NonNullableString JwtSecretKey();
    }
}