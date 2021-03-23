using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MG.Utils.Azure.Blob.Healthcheck
{
    public static class AzureBlobHealthCheckExtensions
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
    }
}