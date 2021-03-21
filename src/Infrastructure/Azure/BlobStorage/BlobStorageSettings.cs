using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Utils.ValueObjects;

namespace WebHost.Infrastructure.Azure.BlobStorage
{
    public class BlobStorageSettings
    {
        private readonly NonNullableString _storageAccount;

        private readonly NonNullableString _storageKey;

        public NonNullableString ConnectionString { get; }

        public NonNullableString FilesContainerName { get; }

        public NonNullableString ImagesContainerName { get; }

        public BlobStorageSettings(IConfiguration configuration)
        {
            var section = configuration
                .GetSection("Azure")
                .GetSection("BlobStorage");

            _storageAccount = new NonNullableString(section["StorageAccount"]);
            _storageKey = new NonNullableString(section["StorageKey"]);
            ConnectionString = new NonNullableString(section["ConnectionString"]);

            var containers = Containers.Create(section);

            FilesContainerName = containers.FilesContainer;
            ImagesContainerName = containers.ImagesContainer;
        }

        public Task<BlobContainerClient> FilesContainerAsync()
        {
            return ContainerAsync(FilesContainerName);
        }

        public Task<BlobContainerClient> ImagesContainerAsync()
        {
            return ContainerAsync(ImagesContainerName);
        }

        private async Task<BlobContainerClient> ContainerAsync(NonNullableString containerName)
        {
            var container = new BlobContainerClient(ConnectionString, containerName);
            await container.CreateIfNotExistsAsync();

            return container;
        }

        private class Containers
        {
            private readonly ICollection<KeyValuePair<string, string>> _containers;

            public Containers(ICollection<KeyValuePair<string, string>> containers)
            {
                _containers = containers;
            }

            public NonNullableString FilesContainer
            {
                get
                {
                    return new (_containers.First(x => x.Key == "Files").Value);
                }
            }

            public NonNullableString ImagesContainer
            {
                get
                {
                    return new (_containers.First(x => x.Key == "Images").Value);
                }
            }

            public static Containers Create(IConfigurationSection section)
            {
                var array = section
                    .GetSection("Containers")
                    .GetChildren()
                    .Select(x =>
                        new KeyValuePair<string, string>(
                            x.GetValue<string>("Key"),
                            x.GetValue<string>("Value")))
                    .ToArray();

                return new Containers(array);
            }
        }
    }
}