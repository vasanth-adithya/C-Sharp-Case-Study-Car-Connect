using System;

namespace CarConnect.Exceptions
{
    internal class AuthenticationException : Exception
    {
        public AuthenticationException() { }

        public AuthenticationException(string message) : base(message) { }
    }
}
