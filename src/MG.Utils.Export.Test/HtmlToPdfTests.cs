using System.Threading.Tasks;
using DinkToPdf.Contracts;
using MG.Utils.Export.PdfServices;
using Moq;
using Xunit;

namespace MG.Utils.Export.Test
{
    public class HtmlToPdfTests
    {
        [Fact]
        public async Task AsByteArray_ReturnsByteArrayAsync()
        {
            var converterMoq = new Mock<IConverter>();
            converterMoq.Setup(x => x.Convert(It.IsAny<IDocument>()))
                .Returns(It.IsAny<byte[]>);

            var htmlToPdf = new HtmlToPdf("<p>Html Content</p>", converterMoq.Object);
            var result = await htmlToPdf.AsByteArrayAsync();
        }
    }
}
