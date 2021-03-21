using System.IO;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Utils.Helpers;

namespace WebHost.Infrastructure.PdfServices
{
    public class PdfContent
    {
        private readonly string _htmlContent;

        public PdfContent(string htmlContent)
        {
            _htmlContent = htmlContent.ThrowIfNull(nameof(htmlContent));
        }

        public async Task<Stream> WriteAsync()
        {
            var workStream = new MemoryStream();

            await using var pdfWriter = new PdfWriter(workStream);
            pdfWriter.SetCloseStream(false);
            using var document = HtmlConverter.ConvertToDocument(_htmlContent, pdfWriter);

            // Returns the written-to MemoryStream containing the PDF.
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }
    }
}