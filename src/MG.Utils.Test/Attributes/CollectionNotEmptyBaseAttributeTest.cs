using System.Collections.Generic;
using MG.Utils.Attributes;
using MG.Utils.Validation;
using MG.Utils.Validation.Exception;
using Xunit;

namespace MG.Utils.Test.Attributes
{
    public class CollectionNotEmptyBaseAttributeTest
    {
        [Fact]
        public void IsValid_CollectionNotEmpty_Ok()
        {
            // No exception is ok
            new Awesome(new int[] { 1, 2, 3 }).ThrowIfInvalid();
        }

        [Fact]
        public void IsValid_CollectionEmpty_Exception()
        {
            Assert.Throws<EntityInvalidException>(() => new Awesome(new int[0]).ThrowIfInvalid());
        }

        [Fact]
        public void IsValid_CollectionNull_Exception()
        {
            Assert.Throws<EntityInvalidException>(() => new Awesome(null).ThrowIfInvalid());
        }

        private class Awesome
        {
            public Awesome(IReadOnlyCollection<int> collection)
            {
                Collection = collection;
            }

            [CollectionNotEmptyBase]
            public IReadOnlyCollection<int> Collection { get; }
        }
    }
}