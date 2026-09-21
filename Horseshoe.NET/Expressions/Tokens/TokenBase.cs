using System;
using System.Collections.Generic;
using System.Text;

using Horseshoe.NET.CodeTrace;
using Horseshoe.NET.Globalization;

namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// The base class for tokens (e.g. strings, dates, scope markers, etc.) used across the expressions subsystem.
    /// </summary>
    public abstract class TokenBase : BaseObj
    {
        public const int ParserPriority_Custom = 0;
        public const int ParserPriority_Whitespace = 10;
        public const int ParserPriority_String = 20;
        public const int ParserPriority_Date = 30;
        public const int ParserPriority_Number = 100;  // e.g. 19.1, π
        public const int ParserPriority_MathematicalConstant = 110;  // e.g. π
        public const int ParserPriority_FractionalNumber = 120;  // e.g. ½
        //public const int ParserPriority_NumberPrefix = 130;  // i.e. -, √
        public const int ParserPriority_Word = 140;  // e.g. keyword, function
        public const int ParserPriority_Scope = 150;  // i.e. '(', ',', ')'
        public const int ParserPriority_Operator = 160;  // e.g. =, +, %, √

        /// <summary>
        /// The original text value parsed from the source text
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        /// The type of token represented by a specific TokenBase submlcass./>
        /// </summary>
        public virtual TokenType Type { get; }

        public int TokenPos { get; }

        /// <summary>
        /// Determines the order in which parsers are run, lower numbers are higher priority.  
        /// Custom parsers should normally use <c>Priority &gt; 30</c> and <c>&lt; 100</c> exclusive (see priority constants in <see cref="TokenBase"/> for more details).
        /// </summary>
        public virtual int Priority { get; }

        /// <summary>
        /// A 2 char-length code unique to each token type in order to string tokens together into phrases.
        /// </summary>
        /// <remarks>
        /// For example, the following combination of tokens "³√27" produces phrase pattern "SPORNL" that can be easily recognized and interpreted by the system.
        /// <list type="bullet">
        /// <item>SP: '³' (superscript)</item>
        /// <item>OR: '√' (operator right-facing)</item>
        /// <item>NL: '27' (numeric literal)</item>
        /// </list>
        /// Other built-in identifiers include.
        /// <list type="bullet">
        /// <item>SB: '(' (scope begin)</item>
        /// <item>SE: ')' (scope end)</item>
        /// <item>SS: ',' (scope separator)</item>
        /// <item>OA: '=', '+' (operator all-facing)</item>
        /// <item>KW: 'HighDate' (keyword)</item>
        /// </list>
        /// </remarks>
        public abstract string PatternIdentifier { get; }

        protected StringBuilder sb { get; } = new StringBuilder();

        /// <summary>
        /// Constructor called via reflection by the parse engine
        /// </summary>
        public TokenBase() 
        {
            RawValue = string.Empty;
        }

        /// <summary>
        /// Constructor used by token instances
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        public TokenBase(string rawValue, int tokenPos = -1)
        {
            RawValue = rawValue?.Trim() ?? string.Empty;
            TokenPos = tokenPos;
        }

        /// <summary>
        /// The main parsing method.  Accepts the current raw <c>char</c> and a host of metadata that allows granular control over the parsing outcome.
        /// Custom parsers must implement this method.
        /// </summary>
        /// <param name="rawSource">The raw source text being parsed into tokens</param>
        /// <param name="pos">The position or index of the current <c>char</c></param>
        /// <param name="tokens">The list storing the tokens parsed up to this point (parsers add identified tokens to this list)</param>
        /// <param name="rawToken">The raw parsed token with which to create the final token instance</param>
        /// <param name="startPos">The starting position of the parsed token in the raw input</param>
        /// <returns><c>true</c> if the current parser handled the current token, <c>false</c> otherwise.</returns>
        public abstract bool Parse
        (
            ReadOnlySpan<char> rawSource,
            ref int pos,
            IEnumerable<TokenBase> tokens,
            out string rawToken,
            out int startPos
        );

        /// <summary>
        /// Creates a new token of the same type as the parser class
        /// </summary>
        /// <param name="rawValue">The parsed raw token</param>
        /// <param name="tokenPos">The <c>0</c>-based position of the parsed token in the original raw input, default is <c>-1</c></param>
        /// <returns>The parsed token</returns>
        public abstract TokenBase CreateInstance(string rawValue, int tokenPos);

        public override string ToString()
        {
            return $"{Type} {{ Pos = {TokenPos}{(this is IValueToken valueToken ? ", Value = " + valueToken.Value.ToDisplayString() : "")} }}";
        }

        private static string traceGroup;
        private static string GetTraceGroup()
        {
            traceGroup ??= typeof(TokenBase).Namespace;
            return traceGroup;
        }

        /// <summary>
        /// Determines whether the supplied character is characterized as whitespace including carriage return, line feed and tab
        /// </summary>
        /// <param name="c">A character</param>
        /// <returns><c>true</c> if character whitespace, otherwise <c>false</c></returns>
        public static bool IsWhitespace(char c) =>
            c.In(' ', '\x00A0', '\t', '\n', '\r');

        /// <summary>
        /// Shows the next character in the raw source text.
        /// </summary>
        /// <param name="rawSource">The raw source text</param>
        /// <param name="pos">The current position in the parse process</param>
        /// <param name="ignoreWhitespace">If <c>true</c>, shows the first non-whitespace character.  Default is <c>false</c>.</param>
        /// <returns>the next character</returns>
        public static char? Next
        (
            ReadOnlySpan<char> rawSource,
            int pos,
            bool ignoreWhitespace = false
        )
        {
            SystemCodeTracing.RelayMethodEntered(paramsAndArgs: new Dictionary<string, object>
            {
                [nameof(rawSource)] = rawSource.ToString(),
                [nameof(pos)] = pos
            }, traceGroup: GetTraceGroup());
            int _pos = pos + 1;
            char c;
            for (; _pos < rawSource.Length; _pos++)
            {
                c = rawSource[_pos];
                if (ignoreWhitespace && c.In(' ', '\x00A0', '\t', '\n', '\r'))
                    continue;
                return SystemCodeTracing.RelayMethodReturningValue(message: "_pos=" + _pos, returnValue: c, traceGroup: GetTraceGroup());
            }
            return SystemCodeTracing.RelayMethodReturningValue(message: "_pos=" + _pos, returnValue: null as char?, traceGroup: GetTraceGroup());
        }

        public static (char?, char?) Next2
        (
            ReadOnlySpan<char> rawSource,
            int pos,
            bool ignoreWhitespace = false
        )
        {
            SystemCodeTracing.RelayMethodEntered(paramsAndArgs: new Dictionary<string, object> 
            { 
                [nameof(rawSource)] = rawSource.ToString(), 
                [nameof(pos)] = pos 
            }, traceGroup: GetTraceGroup());
            int _pos = pos + 1;
            char c;
            char? char0 = null;
            for (; _pos < rawSource.Length; _pos++)
            {
                c = rawSource[_pos];
                if (ignoreWhitespace && c.In(' ', '\x00A0', '\t', '\n', '\r'))
                {
                    SystemCodeTracing.RelayMessage("ignoring whitespace", traceGroup: GetTraceGroup());
                    continue;
                }
                if (char0.HasValue)
                    return SystemCodeTracing.RelayMethodReturningValue(message: "_pos=" + _pos, returnValue: (char0, c), traceGroup: GetTraceGroup());
                else
                    char0 = c;
            }
            return SystemCodeTracing.RelayMethodReturningValue(message: "_pos=" + _pos, returnValue: (char0, null as char?), traceGroup: GetTraceGroup());
        }

        protected static Languages Lang { get; } = new Languages
        {
            { "Token.Word.StartingChar", "A word token may only start with '_' or a letter." },
            { "Token.Parse.Unexpected.{char}.{type}", "Unexpected char '{0}' encountered parsing '{1}' token." },
            { "Token.Parse.Unexpected.{value}.{type}", "Unexpected value '{0}' encountered parsing '{1}' token." },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Token.Word.StartingChar", "Un token de 'Word' solo puede comenzar con '_' o una letra." },
                { "Token.Parse.Unexpected.{char}.{type}", "Carácter inesperado '{0}' encontrado al analizar el token '{1}'." },
                { "Token.Parse.Unexpected.{value}.{type}", "Valor inesperado '{0}' encontrado al analizar el token '{1}'." },
            }
        );
    }
}
