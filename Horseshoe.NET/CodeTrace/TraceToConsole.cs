using System;

namespace Horseshoe.NET.CodeTrace
{
    /// <summary>
    /// A trace listener that can write indented code trace messages to the console and exposes indentation settings.
    /// </summary>
    /// <remarks>Each line is written via Console.WriteLine. The type exposes IndentChar (returns ' ') and
    /// IndentLevel to represent indentation state.</remarks>
    public class TraceToConsole : ITraceListener, IIndentable
    {
        public char IndentChar => ' ';

        /// <inheritdoc cref="IIndentable.IndentLevel"/>
        public int IndentLevel { get; set; }

        /// <inheritdoc cref="IIndentable.IndentWidth"/>
        public int IndentWidth { get; } = 2;

        public TraceToConsole(int? indentWidth = null)
        {
            if (indentWidth.HasValue)
                IndentWidth = indentWidth.Value;
        }

        /// <inheritdoc cref="ITraceListener.Relay(string)"/>
        public void Relay(string line)
        {
            Console.WriteLine(line);
        }
    }
}
