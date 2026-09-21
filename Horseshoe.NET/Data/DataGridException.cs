using System;

namespace Horseshoe.NET.Data
{
    public class DataGridException : Exception
    {
        public DataGridException() : base()
        {
        }

        public DataGridException(string message) : base(message)
        {
        }

        public DataGridException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
