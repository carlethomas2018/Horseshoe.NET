namespace Horseshoe.NET.CodeTrace
{
    /// <summary>
    /// Describes an <see cref="ITraceListener"/> that includes indentation for approximating scope or other hierarchal constructs
    /// </summary>
    public interface IIndentable
    {
        /// <summary>
        /// What to indent with (e.g. ' ')
        /// </summary>
        char IndentChar { get; }

        /// <summary>
        /// Gets the indentation width applied when formatting indented output.
        /// </summary>
        /// <remarks>
        /// Must be non-negative
        /// </remarks>
        int IndentWidth { get; }

        /// <summary>
        /// Gets or sets the indentation level applied when formatting indented output.
        /// </summary>
        /// <remarks>
        /// Must be non-negative
        /// </remarks>
        int IndentLevel { get; set; }
    }
}
