using MG.Utils.Security;
using Xunit;

namespace MG.Utils.Test.Security
{
    public class HashStringTest
    {
        [Theory]
        [InlineData(24, 74)]
        [InlineData(32, 74)]
        public void Random_ForLength_Ok(int length, int expectedLength)
        {
            var token = (string)HashString.Random(length);
            Assert.NotEmpty(token);
            Assert.Equal(expectedLength, token.Length);
        }
    }
}