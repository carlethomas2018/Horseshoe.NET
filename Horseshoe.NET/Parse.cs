using System;
using System.Collections.Generic;
using System.Globalization;

using Horseshoe.NET.Globalization;

namespace Horseshoe.NET
{
    public static class Parse
    {
        private static readonly Dictionary<Type, Func<string, string, NumberStyles, string, DateTimeStyles, object, object>> _parsers = new()
        {
            {
                typeof(sbyte),
                (value, locale, numberStyles, _, _, _) =>
                    sbyte.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(byte),
                (value, locale, numberStyles, _, _, _) =>
                    byte.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(short),
                (value, locale, numberStyles, _, _, _) =>
                    short.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(ushort),
                (value, locale, numberStyles, _, _, _) =>
                    ushort.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(int),
                (value, locale, numberStyles, _, _, _) =>
                    int.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(uint),
                (value, locale, numberStyles, _, _, _) =>
                    uint.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(long),
                (value, locale, numberStyles, _, _, _) =>
                    long.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(ulong),
                (value, locale, numberStyles, _, _, _) =>
                    ulong.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(float),
                (value, locale, numberStyles, _, _, _) =>
                    float.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(double),
                (value, locale, numberStyles, _, _, _) =>
                    double.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(decimal),
                (value, locale, numberStyles, _, _, _) =>
                    decimal.Parse(value, numberStyles, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale))
            },
            {
                typeof(bool),
                (value, locale, _, _, _, _) =>
                {
                    if (value.InIgnoreCase("1", "T", "True", "Y", "Yes", string.Format(locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale), "{0}", true)))
                        return true;
                    if (value.InIgnoreCase("0", "F", "False", "N", "No", string.Format(locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale), "{0}", false)))
                        return false;
                    return bool.Parse(value);
                }
            },
            {
                typeof(DateTime), 
                (value, locale, _, dateFormat, dateTimeStyle, _) => 
                    dateFormat == null 
                        ? DateTime.Parse(value, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale), dateTimeStyle) 
                        : DateTime.ParseExact(value, dateFormat, locale == null ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(locale), dateTimeStyle) 
            }
        };

        public static void RegisterParser(Type type, Func<string, string, NumberStyles, string, DateTimeStyles, object, object> parser)
        {
            _parsers[type] = parser;
        }

        /// <summary>
        /// Parses a string value into the specified type, using the registered parsers. 
        /// </summary>
        /// <typeparam name="T">The type to parse value into</typeparam>
        /// <param name="value">The string value to parse</param>
        /// <param name="defaultValue">The default value to return if parsing fails</param>
        /// <param name="locale">An optional locale to use for parsing date values and numbers</param>
        /// <param name="numberStyle">An optional number style to use for parsing numbers</param>
        /// <param name="dateFormat">An optional date format to use for parsing dates</param>
        /// <param name="dateTimeStyle">An optional date time style to use for parsing date values</param>
        /// <param name="additionalData">An optional object containing additional data to use for parsing</param>
        /// <param name="strict">Indicates whether to throw an exception if no parser is found</param>
        /// <returns>The parsed value, or the default value if parsing fails</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static T Value<T>(string value, T defaultValue = default, string locale = null, NumberStyles numberStyle = NumberStyles.None, string dateFormat = null, DateTimeStyles dateTimeStyle = DateTimeStyles.None, object additionalData = null, bool strict = false) =>
            (T)Value(value, typeof(T), defaultValue, locale, numberStyle, dateFormat, dateTimeStyle, additionalData, strict);

        /// <summary>
        /// Parses a string value into the specified type, using the registered parsers. 
        /// </summary>
        /// <remarks>
        /// If no parser is found for the type, the default value is returned (or an exception is thrown if strict mode is enabled).
        /// </remarks>
        /// <param name="value">The string value to parse</param>
        /// <param name="type">The type to parse the value into</param>
        /// <param name="defaultValue">The default value to return if parsing fails</param>
        /// <param name="locale">An optional locale to use for parsing date values and numbers</param>
        /// <param name="numberStyle">An optional number style to use for parsing numbers</param>
        /// <param name="dateFormat">An optional date format to use for parsing dates</param>
        /// <param name="dateTimeStyle">An optional date time style to use for parsing date values</param>
        /// <param name="additionalData">An optional object containing additional data to use for parsing</param>
        /// <param name="strict">Indicates whether to throw an exception if no parser is found</param>
        /// <returns>The parsed value, or the default value if parsing fails</returns>
        /// <exception cref="NotSupportedException"></exception>
        public static object Value(string value, Type type, object defaultValue = null, string locale = null, NumberStyles numberStyle = NumberStyles.None, string dateFormat = null, DateTimeStyles dateTimeStyle = DateTimeStyles.None, object additionalData = null, bool strict = false)
        {
            if (value == null)
                return defaultValue;

            if (_parsers.TryGetValue(type, out var parser))
                return parser.Invoke(value, locale, numberStyle, dateFormat, dateTimeStyle, additionalData);

            if (strict)
                throw new NotSupportedException(string.Format(Lang.Get("ParserNotFound.{type}"), type));

            return defaultValue;
        }

        private static Languages Lang { get; } = new Languages
        {
            { "ParserNotFound.{type}", "No parser for type '{0}' is registered in the system." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "ParserNotFound.{type}", "No hay un analizador registrado para el tipo '{0}' en el sistema." },
            }
        );
    }
}
