namespace Horseshoe.NET.Expressions.Tokens
{
    /// <summary>
    /// The base class for tokens (e.g. strings, dates, scope markers, etc.) used across the expressions subsystem.
    /// </summary>
    public abstract class TokenBase : BaseObj
    {
        /// <summary>
        /// The original text value parsed from the source text
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        /// The type of token represented by a specific TokenBase subclass./>
        /// </summary>
        public virtual TokenType Type { get; }

        public int TokenPos { get; }

        /// <summary>
        /// A code unique to each token type used to daisy chain tokens together into phrases.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Identifiers of operators and mathemetical constants are verbatim 1-<c>char</c> codes, 
        /// all others are 2-<c>char</c> codes identifying the return type and something reminiscent of the token type.
        /// Client code can override this property with new codes.
        /// </para>
        /// The list of built-in identifiers includes...
        /// <list type="bullet">
        /// <item>TX: text string e.g. "Hello"</item>
        /// <item>TK: text keyword e.g. 'MachineName'</item>
        /// <item>TP: text context-based property e.g. 'MyProperty' (-&gt; <c>new { MyProperty = "Hello" }</c>)</item>
        /// <item>TF: text function e.g. 'If' (-&gt; "If(Age > 50, 'Old', 'Young')")</item>
        /// <item>TG: text group e.g. (-&gt; "LastName + ' Jr.'")</item>
        /// <item>DT: date/time e.g. #1/2/2003#</item>
        /// <item>DK: date keyword e.g. 'HighDate'</item>
        /// <item>DP: date context-based property e.g. 'MyProperty' (-&gt; <c>new { MyProperty = DateTime.Today }</c>)</item>
        /// <item>DF: date function e.g. 'DateAdd' (-&gt; "DateAdd('y', BirthDay, 18)")</item>
        /// <item>DG: date group e.g. (-&gt; "Today + 3")</item>
        /// <item>NB: number e.g. '-.27'</item>
        /// <item>NK: numeric keyword e.g. 'Pi'</item>
        /// <item>NP: numeric context-based property e.g. 'MyProperty' (-&gt; <c>new { MyProperty = MyString.Length }</c>)</item>
        /// <item>NF: numeric function e.g. 'DateDiff' (-&gt; "DateDiff('y', BirthDay, Today)")</item>
        /// <item>NG: numeric group e.g. (-&gt; "3³ + 1⅔")</item>
        /// <item>NR: numeric fraction e.g. '⅔' (-&gt; '2/3')</item>
        /// <item>NS: numeric superscript e.g. '³' (-&gt; '2³' ['2^3'] or '³√27')</item>
        /// <item>BK: boolean keyword e.g. 'True' (-&gt; "HasDiabetes=True")</item>
        /// <item>BP: boolean context-based property e.g. 'HasDiabetes' (-&gt; "HasDiabetes" or "HasDiabetes=True")</item>
        /// <item>BF: boolean function e.g. 'And' (-&gt; "And(Age > 50, HasDiabetes)")</item>
        /// <item>SB,SS,SE: scope begin, separator, end e.g. '(,)' (-&gt; "And(Age > 50, HasDiabetes)")</item>
        /// <item>operators and mathematical constants are verbatim: multiples e.g. +, -, /, *, π are grouped.</item>
        /// </list>
        /// <para>
        /// The combination of tokens <c>"³√27"</c> produces combined identifier <c>"NS√NB"</c> which could be identified in pattern <c>"^NS√N[A-Z]$"</c>.
        /// </para>
        /// </remarks>
        public abstract string PatternIdentifier { get; }

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

        public override string ToString()
        {
            return $"{Type} {{ Pos = {TokenPos}{(this is IValueToken valueToken ? ", Value = " + valueToken.ReturnValue.ToDisplayString() : "")} }}";
        }
    }
}
