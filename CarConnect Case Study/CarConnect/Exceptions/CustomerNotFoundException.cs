using System;

namespace CarConnect.Exceptions
{
    internal class CustomerNotFoundException : ApplicationException
    {
        public CustomerNotFoundException() { }

        public CustomerNotFoundException(string msg) : base(msg) { }
    }
}
