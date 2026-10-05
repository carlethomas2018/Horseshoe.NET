using System;

namespace Horseshoe.NET
{
    /// <summary>
    /// A configuration related exception
    /// </summary>
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message) { }
        //public ConfigException(string message, Exception innerException) : base(message, innerException) { }
    }
}
