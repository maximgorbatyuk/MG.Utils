using MG.Utils.Azure.Blob.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MG.Utils.Azure.Blob.Extensions
{
    public static class AzureBlobExtensions
    {
        public static IServiceCollection AddBlobSettingsService(this IServiceCollection services)
        {
            return services.AddTransient<BlobStorageSettings>();
        }

        public static IServiceCollection AddInMemoryBlobStorage(this IServiceCollection services)
        {
            return services.AddScoped<IBlobStorage, InMemoryBlobStorage>();
        }

        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
        {
            return services.AddScoped<IBlobStorage, AzureBlobStorage>();
        }

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