namespace MG.WebHost.Infrastructure.Contracts.Blob
{
    public class BlobUploadDto
    {
        public BlobUploadDto(int status, string blobName)
        {
            Status = status;
            BlobName = blobName;
        }

        public int Status { get; set; }

        public string BlobName { get; set; }
    }
}