using System;

namespace CarConnect.Exceptions
{
    internal class AdminNotFoundException : ApplicationException
    {
        public AdminNotFoundException() { }

        public AdminNotFoundException(string message) : base(message) { }
    }
}
