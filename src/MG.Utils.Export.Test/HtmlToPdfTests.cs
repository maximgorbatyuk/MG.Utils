using System.Text;
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
            string testingText = "<p>Html Content</p>";
            var converterMoq = new Mock<IConverter>();
            converterMoq.Setup(x => x.Convert(It.IsAny<IDocument>()))
                .Returns(Encoding.ASCII.GetBytes(testingText));

            var htmlToPdf = new HtmlToPdf(testingText, converterMoq.Object);
            var result = await htmlToPdf.AsByteArrayAsync();
            Assert.Equal(testingText, Encoding.ASCII.GetString(result));
        }
    }
}
