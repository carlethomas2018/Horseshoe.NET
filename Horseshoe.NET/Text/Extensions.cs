using System.Linq;
using System.Text;

namespace Horseshoe.NET.Text
{
    /// <summary>
    /// Extension methods for text-related operations.
    /// </summary>
    public static class Extensions
    {
        /// <inheritdoc cref="TextUtil.PadCenter(string, int, char, ExceedsTargetLengthBehavior, string, bool)"/>
        public static string PadCenter(this string text, int totalWidth, char paddingChar = ' ', ExceedsTargetLengthBehavior exceedsBehavior = default, string truncateIndicator = "…", bool nudgeRightIfPaddingIsUneven = false) =>
            TextUtil.PadCenter(text, totalWidth, paddingChar: paddingChar, exceedsBehavior: exceedsBehavior, truncateIndicator: truncateIndicator, nudgeRightIfPaddingIsUneven: nudgeRightIfPaddingIsUneven);

        /// <summary>
        /// Appends a value to a <c>StringBuilder</c> given <c>condition</c> has been met.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="value">a value</param>
        /// <returns>the <c>StringBuilder</c> instance</returns>
        public static StringBuilder Append2(this StringBuilder sb, object value)
        {
            sb.Append(value);
            return sb;
        }

        /// <summary>
        /// Appends a value and a new line to a <c>StringBuilder</c> given <c>condition</c> has been met.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="value">a value</param>
        /// <returns>the <c>StringBuilder</c> instance</returns>
        public static StringBuilder AppendLine2(this StringBuilder sb, object value)
        {
            sb.AppendLine(value?.ToString());
            return sb;
        }

        /// <summary>
        /// Appends a value to a <c>StringBuilder</c> given <c>condition</c> has been met.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="condition">a condition</param>
        /// <param name="value">a value</param>
        /// <returns>the <c>StringBuilder</c> instance</returns>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition)
                sb.Append(value);
            return sb;
        }

        /// <summary>
        /// Appends a value and a new line to a <c>StringBuilder</c> given <c>condition</c> has been met.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="condition">a condition</param>
        /// <param name="value">a value</param>
        /// <returns>the <c>StringBuilder</c> instance</returns>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition)
                sb.AppendLine(value?.ToString());
            return sb;
        }

        /// <summary>
        /// Determines if a <c>StringBuilder</c> contains the supplied <c>char</c>
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="value">a <c>char</c></param>
        /// <param name="ignoreCase">If <c>true</c>, the letter case is ignored during <c>char</c> comparison</param>
        /// <returns><c>true</c> if <c>sb</c> contains <c>value</c></returns>
        public static bool Contains(this StringBuilder sb, char value, bool ignoreCase = false)
        {
            for (int i = 0; i < sb.Length; i++)
            {
                if (ignoreCase ? char.ToUpper(sb[i]) == char.ToUpper(value) : sb[i] == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if a <c>StringBuilder</c> contains the supplied <c>string</c>
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c></param>
        /// <param name="value">a <c>string</c></param>
        /// <param name="ignoreCase">If <c>true</c>, the letter case is ignored during <c>string</c> comparison</param>
        /// <returns><c>true</c> if <c>sb</c> contains <c>value</c></returns>
        public static bool Contains(this StringBuilder sb, string value, bool ignoreCase = false)
        {
            if (value == null)
                return false;
            if (ignoreCase ? sb.ToString().ToUpper().Contains(value.ToUpper()) : sb.ToString().Contains(value))
                return true;
            return false;
        }

        /// <summary>
        /// Returns the first <c>char</c> in a <c>StringBuilder</c>.  If the <c>StringBuilder</c> length is 0 then the ASCII [NUL] <c>char</c> is returned.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c> instance</param>
        /// <returns>the first <c>char</c></returns>
        public static char First(this StringBuilder sb) =>
            sb.Length == 0 ? '\0' : sb[0];

        /// <summary>
        /// Returns the last <c>char</c> in a <c>StringBuilder</c>.  If the <c>StringBuilder</c> length is 0 then the ASCII [NUL] <c>char</c> is returned.
        /// </summary>
        /// <param name="sb">a <c>StringBuilder</c> instance</param>
        /// <returns>the last <c>char</c></returns>
        public static char Last(this StringBuilder sb) =>
            sb.Length == 0 ? '\0' : sb[sb.Length - 1];
    }
}
