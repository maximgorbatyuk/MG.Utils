using MG.Utils.ValueObjects;
using Xunit;

namespace MG.Utils.Test.ValueObjects
{
    public class ArrayParameterTest
    {
        [Fact]
        public void EmptyString_NoItems()
        {
            Assert.Empty(new ArrayParameter(string.Empty));
        }

        [Fact]
        public void Null_NoItems()
        {
            Assert.Empty(new ArrayParameter(null));
        }

        [Fact]
        public void InvalidString_NoItems()
        {
            Assert.Empty(new ArrayParameter("hello world"));
        }

        [Fact]
        public void ValidString_HasItems()
        {
            Assert.Equal(
                new long[] { 1, 2, 3, 4, 5 },
                new ArrayParameter("1,2,3,4,5"));
        }

        [Fact]
        public void ValidString_HAsInvalidItems_HasOnlyValidItems()
        {
            Assert.Equal(
                new long[] { 1, 2, 3, 4, 5 },
                new ArrayParameter("1,2,3,hello,world,4,5"));
        }
    }
}