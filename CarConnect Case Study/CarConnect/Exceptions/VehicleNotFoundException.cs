using System;

namespace CarConnect.Exceptions
{
    internal class VehicleNotFoundException : ApplicationException
    {
        public VehicleNotFoundException() { }

        public VehicleNotFoundException(string message) : base(message) { }
    }
}
