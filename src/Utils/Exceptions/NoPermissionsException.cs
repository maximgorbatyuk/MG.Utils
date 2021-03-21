using System;

namespace Utils.Exceptions
{
    public class NoPermissionsException : Exception
    {
        public NoPermissionsException(string message)
            : base(message)
        {
        }
    }
}