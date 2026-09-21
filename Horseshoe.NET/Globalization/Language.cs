using System;
using System.Collections.Generic;

using Horseshoe.NET.Text;

namespace Horseshoe.NET.Globalization
{
    /// <summary>
    /// Represents a collection of localized values (see property <see cref="Locale"/>).
    /// </summary>
    public class Language : Dictionary<string, string>
    {
        public string Locale { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class with the specified locale.
        /// </summary>
        /// <param name="locale">A string representing the locale.</param>
        /// <exception cref="ArgumentException">Thrown when the locale is null or empty.</exception>
        public Language(string locale)
        {
            Locale = TextUtil.Zap(locale) ?? throw new ArgumentException("Locale cannot be null or empty", nameof(locale));
        }
    }
}
