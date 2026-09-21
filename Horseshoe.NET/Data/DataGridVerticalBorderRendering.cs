namespace Horseshoe.NET.Data
{
    /// <summary>
    /// Represents vertical border areas for grid-to-text partial rendering.
    /// </summary>
    public enum DataGridVerticalBorderRendering
    {
        /// <summary>
        /// No vertical border e.g. no vertical border is configured or a horizontal-only section of border is being rendered
        /// </summary>
        None,

        /// <summary>
        /// The left outer border
        /// </summary>
        Left,

        /// <summary>
        /// Any vertical border minus the left or right
        /// </summary>
        Inner,

        /// <summary>
        /// The right outer border
        /// </summary>
        Right
    }
}
