using MG.Utils.Abstract;
using MG.Utils.Security;
using Xunit;

namespace MG.Utils.Test.Security
{
    public class UserPasswordTest
    {
        [Fact]
        public void AsHash_HashedString_Ok()
        {
            const string password = "Qwerty123$";
            var hash = new UserPasswordHash(password).Value();
            var parsed = new HashedString(hash);
            Assert.True(parsed.Same(password));
        }
    }
}
