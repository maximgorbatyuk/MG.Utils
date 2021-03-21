using System;
using Utils.MathHelpers;
using Xunit;

namespace Utils.Test.Math
{
    public class PercentTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        [InlineData(1000)]
        public void Ctor_InvalidData_Exception(int value)
        {
            Assert.Throws<ArgumentException>(() => new Percent(value));
        }

        [Theory]
        [InlineData(100, 1)]
        [InlineData(90, 0.9)]
        [InlineData(15, 0.15)]
        [InlineData(0, 0)]
        public void AsFraction_ValidSource_Ok(int source, double expected)
        {
            Assert.Equal(expected, new Percent(source).AsFraction());
        }
    }
}