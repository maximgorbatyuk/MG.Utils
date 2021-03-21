using System.IO;
using Azure.Storage.Blobs.Models;

namespace WebHost.Infrastructure.Contracts.Blob
{
    public class BlobDownloadDto
    {
        public BlobDownloadDto(BlobDownloadInfo downloadInfo, string fileName)
        {
            Content = downloadInfo.Content;
            ContentType = downloadInfo.ContentType;
            FileName = fileName;
            ContentLength = downloadInfo.ContentLength;
        }

        public BlobDownloadDto(MemoryStream stream, string blobName, string contentType = "application/pdf")
        {
            Content = stream;
            FileName = blobName;
            ContentType = contentType;
            ContentLength = stream.Length;
        }

        public Stream Content { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }
    }
}