using MG.Utils.Abstract.Exceptions;
using MG.Utils.Attributes;
using MG.Utils.Validation;
using Xunit;

namespace MG.Utils.Test.Attributes
{
    public class ValidEmailAttributeTest
    {
        [Theory]
        [InlineData("admin@petrel.ai")]
        [InlineData("admin123@gmail.com")]
        [InlineData("admin_123@gmail.com")]
        [InlineData("admin_123@жмаил.аф")]
        public void ValidEmail_Ok(string email)
        {
            // No Exception is ok
            new Instance(email).ThrowIfInvalid();
        }

        [Theory]
        [InlineData("     admin@petrel.ai   ")]
        [InlineData("админ@gmail.com")]
        [InlineData("@sdcom")]
        [InlineData("j..s@proseware.com")]
        [InlineData("js*@proseware.com")]
        [InlineData("js@proseware..com")]
        [InlineData("j.@server1.proseware.com")]
        public void InvalidEmail_Exception(string email)
        {
            Assert.Throws<EntityInvalidException>(() => new Instance(email).ThrowIfInvalid());
        }

        private class Instance
        {
            public Instance(string email)
            {
                Email = email;
            }

            [ValidEmail]
            public string Email { get; }
        }
    }
}