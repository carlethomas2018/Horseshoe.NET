using System;

namespace Horseshoe.NET
{
    public class AssertionFailedException : Exception
    {
        public AssertionFailedException(string message) : base(message) { }
    }
}
