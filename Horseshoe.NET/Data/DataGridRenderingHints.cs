namespace Horseshoe.NET.Data
{
    /// <summary>
    /// Settings for rendering a data grid, including options for headers, borders, and padding.
    /// </summary>
    public class DataGridRenderingHints
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the column header row.
        /// </summary>
        public bool ShowColumnHeaderRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to highlight the current row.
        /// </summary>
        public bool HighlightCurrentRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show inner horizontal borders.
        /// </summary>
        public bool InnerHorizontalBorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show inner vertical borders.
        /// </summary>
        public bool InnerVerticalBorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show outer horizontal borders.
        /// </summary>
        public bool OuterHorizontalBorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show outer vertical borders.
        /// </summary>
        public bool OuterVerticalBorders { get; set; }

        /// <summary>
        /// Gets or sets the padding for horizontal borders.
        /// </summary>
        public int HorizontalBorderPadding { get; set; }

        /// <summary>
        /// Gets or sets the padding for vertical borders.
        /// </summary>
        public int VerticalBorderPadding { get; set; }
    }
}
