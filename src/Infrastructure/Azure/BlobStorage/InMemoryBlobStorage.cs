using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utils.Exceptions;
using WebHost.Infrastructure.Contracts.Blob;

namespace WebHost.Infrastructure.Azure.BlobStorage
{
    public class InMemoryBlobStorage : IBlobStorage
    {
        private readonly Dictionary<string, MemoryStream> _storage = new ();

        public int Count => _storage.Count;

        public async Task<BlobUploadDto> UploadAsync(string blobName, Stream data)
        {
            if (_storage.ContainsKey(blobName))
            {
                throw new InvalidOperationException($"The filename {blobName} is contained in blob storage");
            }

            var memory = new MemoryStream();
            await data.CopyToAsync(memory);
            memory.Position = 0;

            _storage.Add(blobName, memory);

            return new BlobUploadDto(200, blobName);
        }

        public Task<BlobDownloadDto> DownloadAsync(string blobName)
        {
            if (Count == 0)
            {
                throw new ResourceNotFoundException("There is no file");
            }

            if (_storage.TryGetValue(blobName, out MemoryStream stream))
            {
                return Task.FromResult(new BlobDownloadDto(stream, blobName));
            }

            throw new ResourceNotFoundException($"No file by name {blobName}");
        }

        public Task<bool> DeleteAsync(string blobName)
        {
            if (_storage.ContainsKey(blobName))
            {
                _storage.Remove(blobName);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}