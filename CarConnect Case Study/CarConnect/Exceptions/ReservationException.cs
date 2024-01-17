using System;

namespace CarConnect.Exceptions
{
    internal class ReservationException : ApplicationException
    {
        public ReservationException() { }

        public ReservationException(string message) : base(message) { }
    }
}
