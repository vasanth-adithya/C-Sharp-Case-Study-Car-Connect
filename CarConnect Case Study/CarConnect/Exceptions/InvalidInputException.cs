using System;

namespace CarConnect.Exceptions
{
    internal class InvalidInputException : Exception
    {
        public InvalidInputException() { }

        public InvalidInputException(string message) : base(message) { }
    }
}
