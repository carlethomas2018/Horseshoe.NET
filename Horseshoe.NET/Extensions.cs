using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Horseshoe.NET.Types;

namespace Horseshoe.NET
{
    /// <summary>
    /// Extension methods for Horseshoe.NET
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Renders an exception as a one-line string e.g. to the console.
        /// </summary>
        /// <param name="ex">An exception</param>
        /// <param name="abbreviate">A value indicating whether to abbreviate the exception type name. Overrides <paramref name="abbreviateIfSystem"/> if <c>true</c>.</param>
        /// <param name="abbreviateIfSystem">A value indicating whether to abbreviate the type name only for exceptions in the System namespace.</param>
        /// <returns>A one-line string representation of the exception</returns>
        public static string Render(this Exception ex, bool abbreviate = false, bool abbreviateIfSystem = false)
        {
            if (abbreviate)
                return string.Format("{0}: {1}", ex.GetType().Name, ex.Message.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "));
            if (abbreviateIfSystem && ex.GetType().FullName.StartsWith("System."))
                return string.Format("{0}: {1}", ex.GetType().Name, ex.Message.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "));
            return string.Format("{0}: {1}", ex.GetType().FullName, ex.Message.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "));
        }

        /// <summary>
        /// Renders an exception e.g. to the console.
        /// </summary>
        /// <param name="ex">An exception</param>
        /// <param name="abbreviate">A value indicating whether to abbreviate the exception type name. Overrides <paramref name="abbreviateIfSystem"/> if <c>true</c>.</param>
        /// <param name="abbreviateIfSystem">A value indicating whether to abbreviate the type name only for exceptions in the System namespace.</param>
        /// <returns>A string representation of the exception with stack trace</returns>

        public static string RenderWithStackTrace(this Exception ex, bool abbreviate = false, bool abbreviateIfSystem = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Render(ex, abbreviate, abbreviateIfSystem));
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="exclusive">If <c>true</c>, will not include matches on min or max.  Default is <c>false</c>.</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this byte value, int min, int max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive 
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this short value, int min, int max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this int value, int min, int max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this long value, long min, long max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this double value, double min, double max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this decimal value, decimal min, decimal max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this DateTime value, DateTime min, DateTime max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this char value, char min, char max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this char value, int min, int max, bool exclusive = false)
        {
            if (min > max)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? value > min && value < max
                : value >= min && value <= max;
        }

        /// <summary>
        /// Determines if a value is between a min and max value
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="ignoreCase">If <c>true</c>, string comparisons do not take letter case into account, default is <c>false</c>.</param>
        /// <returns><c>true</c> if value is between min and max</returns>
        public static bool Between(this string value, string min, string max, bool ignoreCase = false, bool exclusive = false)
        {
            if (string.Compare(min, max, ignoreCase) > 0)
            {
                (max, min) = (min, max);
            }
            return exclusive
                ? string.Compare(value, min, ignoreCase) > 0 && string.Compare(max, value, ignoreCase) > 0
                : string.Compare(value, min, ignoreCase) >= 0 && string.Compare(max, value, ignoreCase) >= 0;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this char value, params char[] criteria)
        {
            return Array.IndexOf(criteria, value) > -1;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this char value, IEnumerable<char> criteria)
        {
            if (criteria == null)
                return false;

            foreach (char c in criteria)
            {
                if (c == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="charValues">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this char value, params int[] charValues)
        {
            return In(value, charValues as IEnumerable<int>);
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="charValues">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this char value, IEnumerable<int> charValues)
        {
            if (charValues == null)
                return false;

            foreach (int c in charValues)
            {
                if (c == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this int value, params int[] criteria)
        {
            return Array.IndexOf(criteria, value) > -1;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this int value, IEnumerable<int> criteria)
        {
            if (criteria == null)
                return false;

            foreach (int i in criteria)
            {
                if (i == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this DateTime value, params DateTime[] criteria)
        {
            return Array.IndexOf(criteria, value) > -1;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this DateTime value, IEnumerable<DateTime> criteria)
        {
            if (criteria == null)
                return false;

            foreach (DateTime dt in criteria)
            {
                if (dt == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this string value, params string[] criteria)
        {
            return Array.IndexOf(criteria, value) > -1;
        }

        /// <summary>
        /// Determines whether the criteria collection contains the supplied value.  Inspired by SQL.
        /// </summary>
        /// <param name="value">A value</param>
        /// <param name="criteria">A collection in which to look up the specified value.</param>
        /// <returns>Returns <c>true</c> if the collections contains the value, otherwise<c>false</c>.</returns>
        public static bool In(this string value, IEnumerable<string> criteria)
        {
            if (criteria == null)
                return false;

            foreach (string s in criteria)
            {
                if (s == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Displays objects in a uniform way e.g. numbers... -3.1, 3.141592652589, others... "Hello world!", "2/12/2022", "System.String"
        /// </summary>
        /// <param name="obj">An object</param>
        /// <returns>A display string</returns>
        public static string ToDisplayString(this object obj)
        {
            if (obj == null)
                return "[null]";

            Type type = obj.GetType();

            if (type.IsNumeric())
                return obj.ToString();

            if (obj is DateTime dateTime)
            {
                obj = dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0
                    ? dateTime.ToShortDateString()
                    : dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }

            else if (obj is MethodBase methodBase)
            {
                obj = methodBase.DeclaringType.Name + "::" + methodBase.Name;
            }

            else  // includes enums
            {
                obj = obj.ToString();
            }
            return "\"" + obj + "\"";
        }
    }
}
