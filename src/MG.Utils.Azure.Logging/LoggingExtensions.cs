using MG.Utils.Abstract.Extensions;
using MG.Utils.Abstract.NonNullableObjects;
using MG.Utils.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace MG.Utils.Azure.Logging
{
    public static class LoggingExtensions
    {
        private const string LogLevelConfigKey = "Logging:LogLevel:Default";
        private const string InstrumentationKeyConfigKey = "ApplicationInsights:InstrumentationKey";

        private static LogLevel LogLevel(IConfiguration config)
        {
            return new NonNullableString(config[LogLevelConfigKey], LogLevelConfigKey)
                .ToString()
                .ToEnum<LogLevel>();
        }

        public static IServiceCollection AddAzureAppInsightsLogs(
            this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            LogLevel level = LogLevel(configuration);
            bool turnOnAppInsights = !isDevelopment;

            services.AddLogging(options =>
            {
                options.AddConsole();
                options.SetMinimumLevel(level);

                if (turnOnAppInsights)
                {
                    // hook the Application Insights Provider
                    options.AddFilter<ApplicationInsightsLoggerProvider>(
                        new NonNullableString(
                            configuration[InstrumentationKeyConfigKey], InstrumentationKeyConfigKey), level);

                    // pass the InstrumentationKey provided under the appsettings
                    options.AddApplicationInsights();
                    options.AddAzureWebAppDiagnostics();
                }
            });

            if (turnOnAppInsights)
            {
                services.AddApplicationInsightsTelemetry();
            }

            return services;
        }
    }
}