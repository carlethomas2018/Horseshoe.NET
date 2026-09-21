using System;

namespace Horseshoe.NET
{
    /// <summary>
    /// An exception that indicates a situation that should never occur in normal operation. 
    /// This exception is typically used to signal a critical error or an unexpected state in the application.
    /// </summary>
    public class ThisShouldNeverHappenException : Exception
    {
        public ThisShouldNeverHappenException() : base("This should never happen.")
        {
        }

        public ThisShouldNeverHappenException(string message) : base(message)
        {
        }
    }
}
