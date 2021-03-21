using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Infrastructure.FileResults
{
    public class XLSFileStreamResult : FileStreamResult
    {
        public XLSFileStreamResult(Stream stream, string name)
            : base(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = name;
        }
    }
}