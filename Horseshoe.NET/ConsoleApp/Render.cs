using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.ConsoleApp
{
    /// <summary>
    /// Provides methods for rendering output to the console.
    /// </summary>
    public static class Render
    {
        /// <summary>
        /// Pads the console output with a specified number of blank lines.
        /// </summary>
        /// <param name="lines">The number of blank lines to add.</param>
        public static void Pad(int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
