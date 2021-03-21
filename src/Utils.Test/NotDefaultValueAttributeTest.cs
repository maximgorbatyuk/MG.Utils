using System;
using System.ComponentModel.DataAnnotations;
using Utils.Attributes;
using Xunit;

namespace Utils.Test
{
    public class NotDefaultValueAttributeTest
    {
        private NotDefaultValueAttribute Target()
        {
            return new NotDefaultValueAttribute();
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(1)]
        [InlineData(1.0)]
        [InlineData(true)]
        [InlineData(AwesomeEnum.NotDefaultValue)]
        public void IsValid_DifferentValueTypes_NotDefault_True(object value)
        {
            Assert.True(Target().IsValid(value));
        }

        [Fact]
        public void IsValid_EnumAsNullableValue_NotDefault_True()
        {
            Assert.True(Target().IsValid(new AwesomeEnum?(AwesomeEnum.NotDefaultValue)));
        }

        [Fact]
        public void IsValid_EnumAsNullableValue_Default_False()
        {
            Assert.False(Target().IsValid(new AwesomeEnum?(AwesomeEnum.Undefined)));
        }

        [Fact]
        public void IsValid_EnumAsNullableValue_Null_True()
        {
            Assert.True(Target().IsValid((AwesomeEnum?)null));
        }

        [Fact]
        public void IsValid_DateTimeOffset_NotDefault_True()
        {
            Assert.True(Target().IsValid(DateTimeOffset.Now));
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(0)]
        [InlineData(0.0)]
        [InlineData(false)]
        [InlineData(AwesomeEnum.Undefined)]
        public void IsValid_DifferentValueTypes_Default_False(object value)
        {
            Assert.False(Target().IsValid(value));
        }

        [Fact]
        public void IsValid_DateTimeOffset_Default_False()
        {
            Assert.False(Target().IsValid(default(DateTimeOffset)));
        }

        [Fact]
        public void IsValid_NotValueType_AnyObject_True()
        {
            Assert.True(Target().IsValid(new AwesomeClass()));
        }

        [Fact]
        public void IsValid_NotValueType_NullAnyObject_True()
        {
            Assert.True(Target().IsValid(null));
        }

        [Fact]
        public void IsValid_String_EmptyString_True()
        {
            Assert.True(Target().IsValid(string.Empty));
        }

        [Fact]
        public void IsValid_String_NotEmptyString_True()
        {
            Assert.True(Target().IsValid("ololo"));
        }

        private enum AwesomeEnum
        {
            Undefined = 0,
            NotDefaultValue = 1
        }

        private class AwesomeClass
        {
        }
    }
}