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
        /// The type of token represented by a specific TokenBase submlcass./>
        /// </summary>
        public virtual TokenType Type { get; }

        public int TokenPos { get; }

        /// <summary>
        /// A 2 char-length code unique to each token type in order to daisy chain tokens together into phrases.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each unique code is comprised of a <c>char</c> identifying the return type and a second <c>char</c> reminiscent of the token type.
        /// Client code can individually override this property and add their own unique codes.
        /// </para>
        /// The list of built-in identifiers includes...
        /// <list type="bullet">
        /// <item>TX: text string e.g. "Hello"</item>
        /// <item>DT: date/time e.g. #1/2/2003#</item>
        /// <item>NB: number e.g. -.27</item>
        /// <item>NC: mathematical constant e.g. π</item>
        /// <item>NF: fraction e.g. ⅔ (-&gt; "2/3")</item>
        /// <item>NS: numeric superscript e.g. ³ (-&gt; "2³" or "2^3")</item>
        /// <item>SB: scope begin e.g. '('</item>
        /// <item>SE: scope end e.g. ')'</item>
        /// <item>SS: scope separator e.g. ','</item>
        /// <item>OA: operator all-facing e.g. =, +</item>
        /// <item>OR: operator right-facing e.g. √ (-&gt; "-√9" or "³√27")</item>
        /// <item>OV: operator varying-facing e.g. - (-&gt; "9-6" or "-√9")</item>
        /// <item>VK: varying return type keyword e.g. HighDate</item>
        /// <item>VP: varying return type context-based property e.g. MyProperty (-&gt; <c>new { MyProperty = "Hello" }</c>)</item>
        /// <item>VF: varying return type function e.g. Not (-&gt; "Not(IsWeekday(Today))")</item>
        /// </list>
        /// <para>
        /// For example, the combination of tokens "³√27" produces phrase pattern "NSORNB" that can be easily recognized by the system and linked to an in-code calculation.
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
            return $"{Type} {{ Pos = {TokenPos}{(this is IValueToken valueToken ? ", Value = " + valueToken.Value.ToDisplayString() : "")} }}";
        }
    }
}
