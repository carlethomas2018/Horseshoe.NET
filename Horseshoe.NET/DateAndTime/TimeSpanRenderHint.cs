namespace Horseshoe.NET.DateAndTime
{
    /// <summary>
    /// Indicates how a <see cref="TimeSpan"/> should be rendered as a string using <see cref="ExtToString(TimeSpan, TimeSpanRenderHint)"/>.
    /// </summary>
    public enum TimeSpanRenderHint
    {
        /// <summary>
        /// Renders all components of the TimeSpan, including zero values.
        /// </summary>
        All,

        /// <summary>
        /// Renders all components of the TimeSpan, excluding leading and trailing zero values.
        /// </summary>
        TrimZeroes,

        /// <summary>
        /// Renders up to two components of the TimeSpan, starting with the first non-zero value.
        /// </summary>
        RuleOfTwo,

        /// <summary>
        /// Renders up to three components of the TimeSpan, starting with the first non-zero value.
        /// </summary>
        RuleOfThree,

        /// <summary>
        /// Renders up to four components of the TimeSpan, starting with the first non-zero value.
        /// </summary>
        RuleOfFour
    }
}
