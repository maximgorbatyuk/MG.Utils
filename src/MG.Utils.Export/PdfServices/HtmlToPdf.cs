using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using MG.Utils.Abstract;

namespace MG.Utils.Export.PdfServices
{
    public class HtmlToPdf
    {
        private readonly string _htmlContent;

        private readonly IConverter _converter;

        public HtmlToPdf(
            string htmlContent,
            IConverter converter)
        {
            _converter = converter;
            _htmlContent = htmlContent.ThrowIfNull(nameof(htmlContent));
        }

        public async Task<byte[]> AsByteArrayAsync()
        {
            var htmlToPdfDocument = GetHtmlToPdfDocument();
            return await Task.Run(() => _converter.Convert(htmlToPdfDocument));
        }

        public byte[] AsByteArray()
        {
            var htmlToPdfDocument = GetHtmlToPdfDocument();
            return _converter.Convert(htmlToPdfDocument);
        }

        protected virtual WebSettings GetWebSettings()
        {
            return new WebSettings
            {
                DefaultEncoding = "utf-8"
            };
        }

        protected virtual ObjectSettings GetObjectSettings()
        {
            return new ObjectSettings
            {
                WebSettings = GetWebSettings(),
                HtmlContent = _htmlContent
            };
        }

        protected virtual HtmlToPdfDocument GetHtmlToPdfDocument()
        {
            return new HtmlToPdfDocument
            {
                Objects =
                {
                    GetObjectSettings()
                },
            };
        }
    }
}