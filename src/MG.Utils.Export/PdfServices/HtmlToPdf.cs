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

        public HtmlToPdf(string htmlContent, IConverter converter)
        {
            _converter = converter;
            _htmlContent = htmlContent.ThrowIfNull(nameof(htmlContent));
        }

        public async Task<byte[]> AsByteArrayAsync(GlobalSettings settings = null)
        {
            var webSettings = new WebSettings
            {
                DefaultEncoding = "utf-8"
            };
            var objectSettings = new ObjectSettings
            {
                WebSettings = webSettings,
                HtmlContent = _htmlContent
            };
            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                Objects = { objectSettings },
                GlobalSettings = settings ?? new GlobalSettings()
            };
            return await Task.Run(() => _converter.Convert(htmlToPdfDocument));
        }
    }
}