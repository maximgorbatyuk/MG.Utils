using MG.Utils.Abstract.NonNullableObjects;
using MG.Utils.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;

namespace MG.Utils.Azure.Authentication
{
    public static class AuthenticationConfigExtensions
    {
        private const string DefaultDownstreamApi = "DownstreamApi";
        private const string DefaultScope = "User.Read";

        // https://dotnetcorecentral.com/blog/authentication-handler-in-asp-net-core/
        public static IServiceCollection AzureActiveDirectorySso(
            this IServiceCollection services,
            IConfigurationSection azureAdConfigSection,
            NonNullableString downstreamApi = null,
            NonNullableString scope = null)
        {
            // https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp
            services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(azureAdConfigSection)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddDownstreamWebApi(downstreamApi ?? DefaultDownstreamApi, x =>
                {
                    x.Scopes = scope ?? DefaultScope;
                })
                .AddDistributedTokenCaches();

            return services;
        }

        public static IServiceCollection AddSelfJwt(
            this IServiceCollection services, NonNullableString jwtSecretKey)
        {
            // https://metanit.com/sharp/aspnet5/23.7.php
            // https://jasonwatmore.com/post/2019/10/14/aspnet-core-3-simple-api-for-authentication-registration-and-user-management
            // https://github.com/dotnet/aspnetcore/issues/4632#issuecomment-444967075
            services
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
                        IssuerSigningKey = new JwtSecretKey(jwtSecretKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}