namespace Horseshoe.NET.Data
{
    /// <summary>
    /// Represents horizontal borders for grid-to-text rendering.  Mainly internal use.
    /// </summary>
    public enum DataGridHorizontalBorderRendering
    {
        /// <summary>
        /// Typically a header or data row, or a horizontal border padding space
        /// </summary>
        None,

        /// <summary>
        /// The top border
        /// </summary>
        Top,

        /// <summary>
        /// Any border minus the top or bottom
        /// </summary>
        Middle,

        /// <summary>
        /// The bottom border
        /// </summary>
        Bottom
    }
}
