using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Horseshoe.NET.Globalization
{
    /// <summary>
    /// Represents a base collection of values localized in U.S. English (i.e., "en-US") as well as a collection of other languages with their localized values. 
    /// </summary>
    public class Languages : Language
    {
        public static string BaseLocale => "en-US";

        public IList<Language> AdditionalLanguages { get; } = new List<Language>();

        private string value;

        public Languages() : base(BaseLocale) { }

        /// <summary>
        /// Adds one or more languages to the collection of additional languages.
        /// </summary>
        /// <param name="languages">An array of languages to add</param>
        /// <returns>The current <see cref="Languages"/> instance</returns>
        public Languages AddLanguages(params Language[] languages)
        {
            if (languages is null)
                return this;

            foreach (var language in languages)
                AddLanguage(language);

            return this;
        }

        private void AddLanguage(Language language)
        {
            if (language is null)
                return;

            if (AdditionalLanguages.Any(l => l.Locale.Equals(language.Locale, StringComparison.CurrentCultureIgnoreCase)))
                throw new ArgumentException($"A language with the locale '{language.Locale}' already exists in the collection.");

            AdditionalLanguages.Add(language);
        }

        /// <summary>
        /// Gets the value for the specified key in the specified locale. 
        /// If the key is not found in the specified locale, it will fallback to the base locale (en-US). 
        /// If the key is not found in either locale, it will return the key itself.
        /// </summary>
        /// <param name="key">A language key</param>
        /// <param name="locale">The locale for which to retrieve the value</param>
        /// <returns>The language value or the key itself if not found</returns>
        public string Get(string key, string locale = null)
        {
            locale ??= CultureInfo.CurrentCulture.Name;

            // Try to get the value for the specified key in the base locale, if applicable
            if (string.Equals(locale, BaseLocale, StringComparison.CurrentCultureIgnoreCase) && TryGetValue(key, out value))
                return value;

            // Try to get the value for the specified key in the specified locale
            if (AdditionalLanguages.FirstOrDefault(l => string.Equals(l.Locale, locale, StringComparison.CurrentCultureIgnoreCase)) is Language language && language.TryGetValue(key, out value))
                return value;

            // If the locale is a two-letter language code (e.g., "es"), try to find a loosely matching locale (e.g., "es-MX")
            if (locale.Length == 2)
            {
                foreach (var lang in AdditionalLanguages)
                {
                    if (lang.Locale.StartsWith(locale, StringComparison.CurrentCultureIgnoreCase) && lang.TryGetValue(key, out value))
                        return value;
                }
            }

            // If the locale is a specific locale (e.g., "es-MX"), try to find a matching language code (e.g., "es")
            foreach (var lang in AdditionalLanguages)
            {
                if (locale.StartsWith(lang.Locale, StringComparison.CurrentCultureIgnoreCase) && lang.TryGetValue(key, out value))
                    return value;
            }

            // Fallback to base locale (en-US) if the key is not found in the specified locale
            if (TryGetValue(key, out value))
                return value;

            // Return the key itself if no language entry is found
            return key;
        }
    }
}
