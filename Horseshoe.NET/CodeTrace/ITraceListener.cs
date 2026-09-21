namespace Horseshoe.NET.CodeTrace
{
    /// <summary>
    /// Describes an object that can display code trace messages to a user or developer
    /// </summary>
    public interface ITraceListener
    {
        /// <summary>
        /// Outputs a code trace message to the preferred medium of the current listener.
        /// </summary>
        /// <param name="line">A code trace message (including decorative lines, whitespace, etc.) or a one-line part of a longer message.</param>
        void Relay(string line);
    }
}
