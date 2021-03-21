using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebHost.Infrastructure.Contracts.FormFiles
{
    public class FileUploadForm
    {
        [Required]
        public virtual string Filename { get; init; }

        [Required]
        public virtual IFormFile File { get; init; }
    }
}