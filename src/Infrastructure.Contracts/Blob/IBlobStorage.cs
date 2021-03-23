using System.IO;
using System.Threading.Tasks;

namespace MG.WebHost.Infrastructure.Contracts.Blob
{
    public interface IBlobStorage
    {
        Task<BlobUploadDto> UploadAsync(string blobName, Stream data);

        Task<BlobDownloadDto> DownloadAsync(string blobName);

        Task<bool> DeleteAsync(string blobName);
    }
}