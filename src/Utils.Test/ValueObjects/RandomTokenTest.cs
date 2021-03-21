using MG.Utils.ValueObjects;
using Xunit;

namespace MG.Utils.Test.ValueObjects
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