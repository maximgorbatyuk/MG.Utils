using Utils.Exceptions;
using Utils.Security;
using Xunit;

namespace Utils.Test.Security
{
    public class ComplexityScoreTest
    {
        [Theory]
        [InlineData("qwerty123!")]
        [InlineData("qwerty1!@")]
        [InlineData("Qwerty123$")]
        public void ValidOrFail_Ok(string pass)
        {
            // No exception is ok
            new ComplexityScore(pass).ValidOrFail();
        }

        [Theory]
        [InlineData("qwertyasd")]
        [InlineData("123243411")]
        [InlineData("!!!@@@##")]
        public void ValidOrFail_NotValid_Exception(string pass)
        {
            Assert.Throws<InputValidationException>(() => new ComplexityScore(pass).ValidOrFail());
        }

        [Fact]
        public void ErrorMessage_SimpleWord_Exception()
        {
            var complexity = new ComplexityScore("qwertyuio");

            Assert.Equal(1, complexity.Total);
            var ex = Assert.Throws<InputValidationException>(() => complexity.ValidOrFail());
            Assert.Equal(
                "The password must contain uppercase letters or numbers or special symbols", ex.Message);
        }

        [Theory]
        [InlineData("Qwertyuio", "The password must contain numbers or special symbols")]
        [InlineData("qwertyuio1", "The password must contain uppercase letters or special symbols")]
        [InlineData("qwertyuio?", "The password must contain uppercase letters or numbers")]
        public void ErrorMessage_Exception(string pass, string message)
        {
            var complexity = new ComplexityScore(pass);

            Assert.Equal(2, complexity.Total);
            var ex = Assert.Throws<InputValidationException>(() => complexity.ValidOrFail());
            Assert.Equal(message, ex.Message);
        }
    }
}