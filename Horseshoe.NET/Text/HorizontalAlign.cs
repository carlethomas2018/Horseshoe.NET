namespace Horseshoe.NET.Text
{
    /// <summary>
    /// Enumeration that specifies horizontal alignment of text.
    /// </summary>
    public enum HorizontalAlign
    {
        /// <summary>
        /// No alignment specified. This is the default value and usually renders as left-aligned.
        /// </summary>
        None,

        /// <summary>
        /// Left-aligned
        /// </summary>
        Left,

        /// <summary>
        /// Center-aligned
        /// </summary>
        Center,

        /// <summary>
        /// Right-aligned
        /// </summary>
        Right,

        /// <summary>
        /// Specifies that content should be expanded to a supplied width, if supported.
        /// </summary>
        Justify
    }
}
