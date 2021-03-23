using HealthChecks.Network.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace MG.WebHost.Infrastructure.Config
{
    public static class AzureInfrastructureConfig
    {
        public static IHealthChecksBuilder AddAzureSmtpHealthCheck(
            this IHealthChecksBuilder services, IConfiguration configuration, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                return services;
            }

            var smtp = new SmtpEmailSettings(configuration);
            services.AddSmtpHealthCheck(
                setup =>
                {
                    setup.Host = smtp.Server;
                    setup.Port = smtp.Port;
                    setup.ConnectionType = SmtpConnectionType.SSL;
                    setup.AllowInvalidRemoteCertificates = true;
                },
                tags: new[] { "smtp" },
                failureStatus: HealthStatus.Degraded);

            return services;
        }
    }
}