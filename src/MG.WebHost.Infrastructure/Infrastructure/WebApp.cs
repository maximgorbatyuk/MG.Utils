using MG.Utils.ValueObjects;
using MG.WebHost.Infrastructure.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MG.WebHost.Infrastructure.Infrastructure
{
    public class WebApp : IWebApp
    {
        private readonly NonNullableString _backendUrl;
        private readonly NonNullableString _frontendUrl;

        public WebApp(IConfiguration config, IHostEnvironment environment)
        {
            _backendUrl = new NonNullableString(config.GetSection("BaseUrls")["Backend"]);
            _frontendUrl = new NonNullableString(config.GetSection("BaseUrls")["Frontend"]);

            IsProduction = environment.IsProduction();
            IsDemo = environment.IsEnvironment("Demo");
            IsStaging = environment.IsStaging();
            IsDevelopment = environment.IsDevelopment();

            EnvironmentName = environment.EnvironmentName;
        }

        public bool IsProduction { get; }

        public bool IsDemo { get; }

        public bool IsStaging { get; }

        public bool IsDevelopment { get; }

        public string EnvironmentName { get; }

        public string BackendAbsoluteUrl(string relativeUrl)
        {
            return $"{_backendUrl.Value()}{Relative(relativeUrl)}";
        }

        public string FrontendAbsoluteUrl(string relativeUrl)
        {
            return $"{_frontendUrl.Value()}{Relative(relativeUrl)}";
        }

        private static string Relative(string url)
        {
            return url.StartsWith("/") ? url : $"/{url}";
        }
    }
}