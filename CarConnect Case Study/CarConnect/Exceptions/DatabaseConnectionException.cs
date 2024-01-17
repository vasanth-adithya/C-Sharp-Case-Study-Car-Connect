using System;

namespace CarConnect.Exceptions
{
    internal class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException() { }

        public DatabaseConnectionException(string message) : base(message) { }
    }
}
