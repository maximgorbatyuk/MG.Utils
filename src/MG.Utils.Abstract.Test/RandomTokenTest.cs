using Xunit;

namespace MG.Utils.Abstract.Test
{
    public class RandomTokenTest
    {
        [Fact]
        public void RandomToken_Ok()
        {
            var token = (string)new RandomToken();
            Assert.NotEmpty(token);
            Assert.Equal(64, token.Length);
        }
    }
}