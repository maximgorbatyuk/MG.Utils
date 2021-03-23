using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;
using MG.WebHost.Infrastructure.Contracts.Blob;

namespace MG.WebHost.Infrastructure.Azure.BlobStorage
{
    // https://github.com/Azure/azure-sdk-for-net/blob/Azure.Storage.Blobs_12.8.0/sdk/storage/Azure.Storage.Blobs/README.md
    // https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
    public class AzureBlobStorage : IBlobStorage
    {
        private readonly BlobStorageSettings _settings;

        public AzureBlobStorage(BlobStorageSettings settings)
        {
            _settings = settings;
        }

        public async Task<BlobUploadDto> UploadAsync(string blobName, Stream data)
        {
            var client = await _settings.FilesContainerAsync();
            var response = await client.UploadBlobAsync(blobName, data);

            return new BlobUploadDto(response.GetRawResponse().Status, blobName);
        }

        public async Task<BlobDownloadDto> DownloadAsync(string blobName)
        {
            var client = await _settings.FilesContainerAsync();

            Response<BlobDownloadInfo> data = await client.GetBlobClient(blobName).DownloadAsync();

            return new BlobDownloadDto(data.Value, blobName);
        }

        public async Task<bool> DeleteAsync(string blobName)
        {
            var client = await _settings.FilesContainerAsync();
            return await client.GetBlobClient(blobName).DeleteIfExistsAsync();
        }
    }
}