namespace Horseshoe.NET.Expressions.Functions
{
    public class Substring : FunctionBase
    {
        /// <summary>
        /// Returns a substring of the original supplied text <c>string</c>.
        /// </summary>
        /// <param name="arg0">A text string</param>
        /// <param name="arg1">The 0-based starting character position of a substring of the original text <c>string</c></param>
        /// <param name="arg2">The optional number of characters in the substring.  If omitted, the resulting substring will end on the last character of the original text <c>string</c>.</param>
        /// <returns>a substring of the original <c>string</c></returns>
        public string Execute(string arg0, int arg1, int arg2 = -1)
        {
            return arg2 < 0
                ? arg0.Substring(arg1)
                : arg0.Substring(arg1, arg2);
        }
    }
}
