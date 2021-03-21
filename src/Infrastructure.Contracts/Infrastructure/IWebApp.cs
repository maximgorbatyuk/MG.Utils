namespace WebHost.Infrastructure.Contracts.Infrastructure
{
    public interface IWebApp
    {
        bool IsProduction { get; }

        bool IsDemo { get; }

        bool IsStaging { get; }

        bool IsDevelopment { get; }

        string EnvironmentName { get; }

        string BackendAbsoluteUrl(string relativeUrl);

        string FrontendAbsoluteUrl(string relativeUrl);
    }
}