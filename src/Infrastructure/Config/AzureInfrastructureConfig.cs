using HealthChecks.Network.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using WebHost.Infrastructure.Azure.BlobStorage;
using WebHost.Infrastructure.Emails;

namespace WebHost.Infrastructure.Config
{
    public static class AzureInfrastructureConfig
    {
        public static IHealthChecksBuilder AddAzureBlobHealthCheck(
            this IHealthChecksBuilder services, IConfiguration configuration)
        {
            var settings = new BlobStorageSettings(configuration);

            services
                .AddAzureBlobStorage(
                    connectionString: settings.ConnectionString,
                    containerName: settings.FilesContainerName,
                    name: $"Azure Blob: {settings.FilesContainerName}")
                .AddAzureBlobStorage(
                    connectionString: settings.ConnectionString,
                    containerName: settings.FilesContainerName,
                    name: $"Azure Blob: {settings.ImagesContainerName}");

            return services;
        }

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