using MG.Utils.Abstract.NonNullableObjects;
using MG.Utils.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace MG.WebHost.Infrastructure.Logging
{
    public class Logging
    {
        private readonly IConfiguration _config;
        private readonly IServiceCollection _services;
        private readonly IHostEnvironment _env;

        public Logging(IConfiguration configuration, IServiceCollection services, IHostEnvironment env)
        {
            _config = configuration;
            _services = services;
            _env = env;
        }

        private LogLevel LogLevel()
        {
            return ((string)new NonNullableString(_config["Logging:LogLevel:Default"])).ToEnum<LogLevel>();
        }

        public void Setup()
        {
            LogLevel level = LogLevel();
            bool turnOnAppInsights = !_env.IsDevelopment();

            _services.AddLogging(options =>
            {
                options.AddConsole();
                options.SetMinimumLevel(level);

                if (turnOnAppInsights)
                {
                    // hook the Application Insights Provider
                    options.AddFilter<ApplicationInsightsLoggerProvider>(
                        new NonNullableString(_config["ApplicationInsights:InstrumentationKey"]), level);

                    // pass the InstrumentationKey provided under the appsettings
                    options.AddApplicationInsights();
                    options.AddAzureWebAppDiagnostics();
                }
            });

            if (turnOnAppInsights)
            {
                _services.AddApplicationInsightsTelemetry();
            }
        }
    }
}