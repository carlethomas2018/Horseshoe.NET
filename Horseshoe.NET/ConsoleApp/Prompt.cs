using System;
using System.Collections.Generic;
using System.Text;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.ConsoleApp
{
    /// <summary>
    /// Provides methods for reading input from the console.
    /// </summary>
    public static class Prompt
    {
        public static string ReadLine(string prompt = ">", bool maskInput = false, string maskChar = "*")
        {
            Console.Write(prompt);
            if (maskInput)
            {
                var input = new StringBuilder();
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                    {
                        input.Append(keyInfo.KeyChar);
                        Console.Write(maskChar);
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input.Length -= 1;
                        Console.Write("\b \b");
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);
                Console.WriteLine();
                return input.ToString();
            }
            else
            {
                return Console.ReadLine();
            }
        }

        /// <summary>
        /// Diplays a message prompting the user to press any key to continue, and waits for a key press.
        /// </summary>
        /// <param name="prompt">An optional message to display to the user.</param>
        /// <param name="padBefore">The number of blank lines to display before the message.</param>
        /// <param name="padAfter">The number of blank lines to display after the message.</param>
        /// <param name="locale">The locale to use for the message.</param>
        public static void Continue(string prompt = null, int padBefore = 1, int padAfter = 1, string locale = null)
        {
            prompt ??= Lang.Get("Prompt.Continue", locale: locale);
            Render.Pad(padBefore);
            Console.WriteLine(prompt);
            Console.ReadKey(intercept: true);
            Render.Pad(padAfter);
        }

        private static Languages Lang { get; } = new Languages
        {
            { "Prompt.Continue", "Press any key to continue..." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Prompt.Continue", "Presione cualquier tecla para continuar..." },
            }
        );
    }
}
