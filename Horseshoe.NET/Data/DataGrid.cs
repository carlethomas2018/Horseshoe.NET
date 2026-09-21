using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Horseshoe.NET.Collections;
using Horseshoe.NET.DateAndTime;
using Horseshoe.NET.Globalization;
using Horseshoe.NET.Text;
using Horseshoe.NET.Types;

namespace Horseshoe.NET.Data
{
    public class DataGrid : List<DataRow>
    {
        /// <summary>
        /// The grid name
        /// </summary>
        public string Name { get; set; }

        private readonly List<DataColumn> columns;

        /// <summary>
        /// The grid columns
        /// </summary>
        public DataColumn[] Columns => columns.ToArray();

        /// <summary>
        /// The grid column names
        /// </summary>
        public string[] ColumnNames => columns.Select(c => c.Name).ToArray();

        /// <summary>
        /// The count of grid columns
        /// </summary>
        public int ColumnCount => columns.Count;

        /// <summary>
        /// The current row number
        /// </summary>
        public int Row { get; set; } = -1;

        /// <summary>
        /// The count of data rows in the grid
        /// </summary>
        public int RowCount => this.Count;

        /// <summary>
        /// The crrent row
        /// </summary>
        public DataRow CurrentRow => (Row >= 0 && Row < RowCount) ? this[Row] : null;

        private List<string[]> prerenderedGrid;

        /// <summary>
        /// Settings for rendering a data grid, including options for headers, borders, and padding.
        /// </summary>
        public DataGridRenderingHints RenderingHints { get; set; }

        private readonly StringBuilder tempSb;

        /// <summary>
        /// An optional format to use for <c>null</c> values during grid rendering, default is <c>""</c>.
        /// </summary>
        public string NullFormat { get; set; }

        public DataGrid(string name = null, IEnumerable<DataColumn> columns = null)
        {
            Name = name;
            this.columns = new List<DataColumn>();
            if (columns != null)
                foreach (var col in columns)
                    AddColumn(col);
            RenderingHints = new DataGridRenderingHints();
            tempSb = new StringBuilder();
        }

        /// <summary>
        /// Adds a column to the DataGrid. If the DataGrid already has rows, each row will be fitted to the new column count.
        /// </summary>
        /// <param name="name">The name of the column to add.</param>
        /// <param name="dataType">The data type of the column.</param>
        /// <param name="format">The format string for the column.</param>
        /// <param name="nullFormat">The format string for null values in the column.</param>
        /// <param name="align">The horizontal alignment for the column.</param>
        /// <param name="headerAlign">The horizontal alignment for the column header.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        public DataGrid AddColumn(string name = null, Type dataType = null, string format = null, string nullFormat = null, HorizontalAlign align = HorizontalAlign.None, HorizontalAlign headerAlign = HorizontalAlign.None, object value = null)
        {
            return AddColumn(new DataColumn { Name = name, DataType = dataType, Format = format, NullFormat = nullFormat, Align = align, HeaderAlign = headerAlign }, value);
        }

        /// <summary>
        /// Adds a column to the DataGrid. If the DataGrid already has rows, each row will be fitted to the new column count.
        /// </summary>
        /// <typeparam name="T">The data type of the column.</typeparam>
        /// <param name="name">The name of the column to add.</param>
        /// <param name="format">The format string for the column.</param>
        /// <param name="nullFormat">The format string for null values in the column.</param>
        /// <param name="align">The horizontal alignment for the column.</param>
        /// <param name="headerAlign">The horizontal alignment for the column header.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        public DataGrid AddColumn<T>(string name = null, string format = null, string nullFormat = null, HorizontalAlign align = HorizontalAlign.None, HorizontalAlign headerAlign = HorizontalAlign.None, T value = default)
        {
            return AddColumn(new DataColumn { Name = name, DataType = typeof(T), Format = format, NullFormat = nullFormat, Align = align, HeaderAlign = headerAlign }, value);
        }

        /// <summary>
        /// Adds a column to the DataGrid. If the DataGrid already has rows, each row will be fitted to the new column count.
        /// </summary>
        /// <param name="column">A new column.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        public DataGrid AddColumn(DataColumn column, object value = null)
        {
            columns.Add(column);
            for (int r = 0; r < this.Count; r++)
            {
                ListUtil.Fit(this[r], ColumnCount, value: value);
            }
            return this;
        }

        /// <summary>
        /// Inserts a column into the DataGrid. If the DataGrid already has rows, a default value will be inserted into each row.
        /// </summary>
        /// <param name="index">The index at which to insert the column.</param>
        /// <param name="name">The name of the column to insert.</param>
        /// <param name="dataType">The data type of the column.</param>
        /// <param name="format">The format string for the column.</param>
        /// <param name="nullFormat">The format string for null values in the column.</param>
        /// <param name="align">The horizontal alignment for the column.</param>
        /// <param name="headerAlign">The horizontal alignment for the column header.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void InsertColumn(int index, string name = null, Type dataType = null, string format = null, string nullFormat = null, HorizontalAlign align = HorizontalAlign.None, HorizontalAlign headerAlign = HorizontalAlign.None, object value = null)
        {
            InsertColumn(index, new DataColumn { Name = name, DataType = dataType, Format = format, NullFormat = nullFormat, Align = align, HeaderAlign = headerAlign }, value);
        }

        /// <summary>
        /// Inserts a column into the DataGrid. If the DataGrid already has rows, a default value will be inserted into each row.
        /// </summary>
        /// <typeparam name="T">The data type of the column.</typeparam>
        /// <param name="index">The index at which to insert the column.</param>
        /// <param name="name">The name of the column to insert.</param>
        /// <param name="format">The format string for the column.</param>
        /// <param name="nullFormat">The format string for null values in the column.</param>
        /// <param name="align">The horizontal alignment for the column.</param>
        /// <param name="headerAlign">The horizontal alignment for the column header.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void InsertColumn<T>(int index, string name = null, string format = null, string nullFormat = null, HorizontalAlign align = HorizontalAlign.None, HorizontalAlign headerAlign = HorizontalAlign.None, T value = default)
        {
            InsertColumn(index, new DataColumn { Name = name, DataType = typeof(T), Format = format, NullFormat = nullFormat, Align = align, HeaderAlign = headerAlign }, value);
        }

        /// <summary>
        /// Inserts a column into the DataGrid. If the DataGrid already has rows, a default value will be inserted into each row.
        /// </summary>
        /// <param name="index">The index at which to insert the column.</param>
        /// <param name="column">A new column.</param>
        /// <param name="value">The value to initialize each cell in the new column with.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void InsertColumn(int index, DataColumn column, object value = null)
        {
            if (index < 0 || index > ColumnCount)
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(Lang.Get("Column.Index.0.{count}"), ColumnCount));  // e.g. "Index must be between 0 and {0}"

            columns.Insert(index, column);
            for (int r = 0; r < this.Count; r++)
            {
                this[r].Insert(index, value);
            }
        }

        /// <summary>
        /// Removes a column from the DataGrid. If the DataGrid has rows, each row will be adjusted to the new column count.
        /// </summary>
        /// <param name="index">The index at which to delete the column.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveColumn(int index)
        {
            if (index < 0 || index > ColumnCount)
                throw new ArgumentOutOfRangeException(nameof(index), string.Format(Lang.Get("Column.Index.0.{count}", locale: null), ColumnCount));  // e.g. "Index must be between 0 and {0}"

            columns.RemoveAt(index);
            for (int r = 0; r < this.Count; r++)
            {
                this[r].RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes a column from the DataGrid. If the DataGrid has rows, each row will be adjusted to the new column count.
        /// </summary>
        /// <param name="columnName">The name of the column to delete.</param>
        /// <exception cref="DataGridException"></exception>
        public void RemoveColumn(string columnName)
        {
            FindColumnIndex(columnName, out int columnIndex);

            RemoveColumn(columnIndex);
        }

        internal void FindColumnIndex(string columnName, out int columnIndex)
        {
            for (int i = 0; i < Columns.Length; i++)
            {
                if (string.Equals(Columns[i].Name, columnName, StringComparison.OrdinalIgnoreCase))
                {
                    columnIndex = i;
                    return;
                }
            }
            throw new DataGridException(string.Format(Lang.Get("Column.NotFound.{name}", locale: null), columnName));  // e.g. "Column not found: {0}"
        }

        /// <summary>
        /// Add and select a new data row
        /// </summary>
        /// <param name="values">An optional array of values to initialize the new row with.</param>
        /// <returns>The updated DataGrid.</returns>
        public DataGrid AddRow(params object[] values)
        {
            values ??= new object[0];
            var list = new List<object>(ColumnCount);
            
            for (int c = 0; c < ColumnCount; c++)
            {
                if (values.Length > c)
                {
                    if (values[c] == null)
                        list.Add(TypeUtil.GetDefaultValue(columns[c].DataType));
                    if (!columns[c].DataType.IsAssignableFrom(values[c].GetType()))
                        throw new DataGridException(string.Format(Lang.Get("Rows.DataMismatch.{columnType}.{dataType}"), Columns[c].Name, Columns[c].DataType.ToShortName(), values[c].GetType().ToShortName()));
                    if (columns[c].DataType == typeof(DateTime) && DateTimeConstants.PreferBusinessDates)
                    {
                        list.Add(((DateTime)values[c]).Wrangle());
                        continue;
                    }
                    list.Add(values[c]);
                }
                else
                {
                    list.Add(TypeUtil.GetDefaultValue(columns[c].DataType));
                }
            }

            var row = new DataRow(list)
            {
                Parent = this
            };
            this.Add(row);
            Row = this.Count - 1;
            return this;
        }

        /// <summary>
        /// Renders the value of a cell in the DataGrid, applying any specified format for the column.
        /// </summary>
        /// <param name="row">The data row</param>
        /// <param name="col">The column index</param>
        /// <returns>The rendered string</returns>
        public string Render(int row, int col)
        {
            DataColumn dataCol = Columns[col];

            if (this[row][col] == null)
                return dataCol.NullFormat ?? NullFormat ?? string.Empty;

            return string.IsNullOrEmpty(dataCol.Format)
                ? this[row][col].ToString()
                : string.Format("{0:" + dataCol.Format + "}", this[row][col]);
        }

        /// <summary>
        /// Renders the value of a cell in the current row of the DataGrid, applying any specified format for the column.
        /// </summary>
        /// <param name="col">The column index</param>
        /// <returns>The rendered string</returns>
        public string Render(int col) =>
            Render(Row, col);

        /// <summary>
        /// Renders the entire DataGrid with optional rendering hints.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Example 1: Rendering data grid with headers
        /// <code>
        /// vertical border padding = 1 added by default if not rendering vertical borders
        ///        ↓
        /// Integer String    Integer│String  // with horizontal and vertical inner borders
        ///       1 Hello     ───────┼──────
        ///       2 World           1│Hello 
        ///                   ───────┼──────
        ///                         2│World 
        /// </code>
        /// </para>
        /// <para>
        /// Example 2: Render with inner and outer borders
        /// <code>
        ///                        vertical borders
        ///                      (rendered partially)
        ///                      ↓         ↓        ↓
        /// horizontal borders → ╔═════════╦════════╗
        ///  (rendered whole)    ║         ║        ║
        ///                      │ Integer │ String │ ← column header row
        /// ┌───────┬──────┐     │         │        │
        /// │Integer│String│   → ├─────────┼────────┤
        /// ├───────┼──────┤     │         │        │
        /// │      1│Hello │     │ ░░░░░░1 │ Hello░ │ ← data row
        /// ├───────┼──────┤     │         │        │
        /// │      2│World │   → ├─────────┼────────┤    
        /// └───────┴──────┘     │ ░░░░░░░ │ ░░░░░░ │ ← horizontal border padding (rendered like any other row)
        ///                      │       2 │ World  │
        ///                      │░       ░│░      ░│
        ///                    → └─────────┴────────┘
        ///                       ↑       ↑ ↑      ↑
        ///                     vertical border padding (spaces)
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="renderingHints"></param>
        /// <returns></returns>
        public string RenderGrid(DataGridRenderingHints renderingHints = null)
        {
            var sb = new StringBuilder();
            renderingHints ??= RenderingHints;
            prerenderedGrid = PrerenderGrid(renderingHints);
            DataColumn dataCol;

            for (int c = 0; c < ColumnCount; c++)
            {
                dataCol = Columns[c];
                dataCol.CalculatedWidth = dataCol.Name.Length;
                for (int r = 0; r < RowCount; r++)
                {
                    dataCol.CalculatedWidth = Math.Max(dataCol.CalculatedWidth, prerenderedGrid[r][c].Length);
                }
            }

            if (renderingHints.ShowColumnHeaderRow)
            {
                if (renderingHints.OuterHorizontalBorders)
                    RenderHorizontalBorder(sb, renderingHints, horizontalBorder: DataGridHorizontalBorderRendering.Top);
                RenderColumnHeaderRow(sb, renderingHints);
                if (renderingHints.InnerHorizontalBorders)
                    RenderHorizontalBorder(sb, renderingHints, horizontalBorder: DataGridHorizontalBorderRendering.Middle);
            }
            else if (renderingHints.OuterVerticalBorders)
            {
                RenderHorizontalBorder(sb, renderingHints, horizontalBorder: DataGridHorizontalBorderRendering.Top);
            }

            if (this.Count > 0)
            {
                for(int r = 0; r < RowCount; r++)
                {
                    if (r > 0 && renderingHints.InnerHorizontalBorders)
                    {
                        RenderHorizontalBorder(sb, renderingHints, horizontalBorder: DataGridHorizontalBorderRendering.Middle);
                    }

                    RenderDataRow(prerenderedGrid[r], sb, renderingHints, r);
                }
            }
            else
            {
                RenderDataRow_NoData(sb, renderingHints);
            }

            if (renderingHints.OuterHorizontalBorders)
                RenderHorizontalBorder(sb, renderingHints, horizontalBorder: DataGridHorizontalBorderRendering.Bottom);
            
            return sb.ToString();
        }

        public List<string[]> PrerenderGrid(DataGridRenderingHints renderingHints = null)
        {
            var list = new List<string[]>();
            for (int r = 0; r < RowCount; r++)
            {
                var row = new string[ColumnCount];
                for (int c = 0; c < ColumnCount; c++)
                {
                    row[c] = Render(r, c);
                }
                list.Add(row);
            }

            for (int c = 0; c < ColumnCount; c++)
            {
                Columns[c].CalculatedWidth = renderingHints.ShowColumnHeaderRow ? Columns[c].Name.Length : 1;
                for (int r = 0; r < RowCount; r++)
                {
                    Columns[c].CalculatedWidth = Math.Max(Columns[c].CalculatedWidth, list[r][c].Length);
                }
            }

            for (int c = 0; c < ColumnCount; c++)
            {
                Columns[c].RenderedName = Columns[c].HeaderAlign switch
                {
                    HorizontalAlign.Center => TextUtil.PadCenter(Columns[c].Name, Columns[c].CalculatedWidth),
                    HorizontalAlign.Right => Columns[c].Name.PadLeft(Columns[c].CalculatedWidth),
                    _ => Columns[c].Name.PadRight(Columns[c].CalculatedWidth)  // none or left
                };

                for (int r = 0; r < RowCount; r++)
                {
                    list[r][c] = Columns[c].Align switch
                    {
                        HorizontalAlign.Center => TextUtil.PadCenter(list[r][c], Columns[c].CalculatedWidth),
                        HorizontalAlign.Right => list[r][c].PadLeft(Columns[c].CalculatedWidth),
                        _ => list[r][c].PadRight(Columns[c].CalculatedWidth)  // none or left
                    };
                }
            }

            return list;
        }

        private void RenderColumnHeaderRow(StringBuilder sb, DataGridRenderingHints renderingHints)
        {
            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Left, renderingHints);

            for (int c = 0; c < ColumnCount; c++)
            {
                if (c > 0)
                {
                    RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Inner, renderingHints);
                }
                sb.Append(Columns[c].RenderedName);
            }

            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Right, renderingHints);

            sb.AppendLine();
        }

        private void RenderDataRow(string[] renderedRowData, StringBuilder sb, DataGridRenderingHints renderingHints, int row)
        {
            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Left, renderingHints, row);

            for (int c = 0; c < ColumnCount; c++)
            {
                if (c > 0)
                {
                    RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Inner, renderingHints);
                }
                sb.Append(renderedRowData[c]);
            }

            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Right, renderingHints, row);

            sb.AppendLine();
        }

        private void RenderDataRow_NoData(StringBuilder sb, DataGridRenderingHints renderingHints)
        {
            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Left, renderingHints);

            int totalWidth = Columns.Sum(c => c.CalculatedWidth) + (ColumnCount - 1) * GetLength_Partial_VerticalBorder(renderingHints, DataGridHorizontalBorderRendering.Middle, DataGridVerticalBorderRendering.Inner);
            sb.AppendLine(Lang.Get("Rows.NoData", locale: null).PadRight(totalWidth));  // e.g. "No data"

            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Right, renderingHints);

            sb.AppendLine();
        }

        private void RenderHorizontalBorder(StringBuilder sb, DataGridRenderingHints renderingHints, DataGridHorizontalBorderRendering horizontalBorder)
        {
            switch (horizontalBorder)
            {
                case DataGridHorizontalBorderRendering.None:
                    return;
                case DataGridHorizontalBorderRendering.Top:
                case DataGridHorizontalBorderRendering.Bottom:
                    if (!renderingHints.OuterHorizontalBorders)
                        return;
                    break;
                case DataGridHorizontalBorderRendering.Middle:
                    if (!renderingHints.InnerHorizontalBorders)
                        return;
                    break;
                default:
                    throw new ThisShouldNeverHappenException($"Unhandled horizontal border type: {horizontalBorder}");
            }

            switch (horizontalBorder)
            {
                case DataGridHorizontalBorderRendering.Bottom:
                case DataGridHorizontalBorderRendering.Middle:
                    for (int p = 0; p < renderingHints.HorizontalBorderPadding; p++)
                        RenderPaddingRow(sb, renderingHints);
                    break;
            }

            RenderPartial_Border(sb, horizontalBorder, DataGridVerticalBorderRendering.Left, renderingHints);

            for (int c = 0; c < ColumnCount; c++)
            {
                if (c > 0)
                {
                    RenderPartial_Border(sb, horizontalBorder, DataGridVerticalBorderRendering.Inner, renderingHints);
                }
                sb.Append(new string('─', Columns[c].CalculatedWidth));
            }

            RenderPartial_Border(sb, horizontalBorder, DataGridVerticalBorderRendering.Right, renderingHints);

            sb.AppendLine();

            switch (horizontalBorder)
            {
                case DataGridHorizontalBorderRendering.Top:
                case DataGridHorizontalBorderRendering.Middle:
                    for (int p = 0; p < renderingHints.HorizontalBorderPadding; p++)
                        RenderPaddingRow(sb, renderingHints);
                    break;
            }
        }

        private void RenderPaddingRow(StringBuilder sb, DataGridRenderingHints renderingHints)
        {
            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Left, renderingHints);

            for (int c = 0; c < ColumnCount; c++)
            {
                if (c > 0)
                {
                    RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Inner, renderingHints);
                }
                sb.Append(new string(' ', Columns[c].CalculatedWidth));
            }

            RenderPartial_Border(sb, DataGridHorizontalBorderRendering.None, DataGridVerticalBorderRendering.Right, renderingHints);

            sb.AppendLine();
        }

        private int GetLength_Partial_VerticalBorder(DataGridRenderingHints renderingHints, DataGridHorizontalBorderRendering horizontalBorder, DataGridVerticalBorderRendering verticalBorder)
        {
            tempSb.Clear();
            RenderPartial_Border(tempSb, horizontalBorder, verticalBorder, renderingHints);
            return tempSb.Length;
        }

        private void RenderPartial_Border(StringBuilder sb, DataGridHorizontalBorderRendering horizontalBorder, DataGridVerticalBorderRendering verticalBorder, DataGridRenderingHints renderingHints, int row = -1)
        {
            switch (verticalBorder)
            {
                case DataGridVerticalBorderRendering.None:

                    switch (horizontalBorder)
                    {
                        case DataGridHorizontalBorderRendering.None:
                            break;
                        case DataGridHorizontalBorderRendering.Top:
                        case DataGridHorizontalBorderRendering.Middle:
                        case DataGridHorizontalBorderRendering.Bottom:
                            sb.Append('─');
                            break;
                        default:
                            throw new ThisShouldNeverHappenException($"Unhandled horizontal border type: {horizontalBorder}");
                    }

                    break;

                case DataGridVerticalBorderRendering.Left:

                    if (renderingHints.HighlightCurrentRow)
                    {
                        sb.Append(row == Row ? '>' : ' ');
                    }

                    if (!renderingHints.OuterVerticalBorders)
                        return; 

                    switch (horizontalBorder)
                    {
                        case DataGridHorizontalBorderRendering.None:
                            sb.Append('│');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string(' ', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Top:
                            sb.Append('┌');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Middle:
                            sb.Append('├');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Bottom:
                            sb.Append('└');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        default:
                            throw new ThisShouldNeverHappenException($"Unhandled horizontal border type: {horizontalBorder}");
                    }

                    break;

                case DataGridVerticalBorderRendering.Inner:

                    if (!renderingHints.InnerVerticalBorders)
                    {
                        sb.Append(' ');  // if no inner vertical borders, always add a space (or padding) between columns for readability
                        break;
                    }
                        
                    switch (horizontalBorder)
                    {
                        case DataGridHorizontalBorderRendering.None:
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string(' ', renderingHints.VerticalBorderPadding));
                            sb.Append('│');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string(' ', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Top:
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            sb.Append('┬');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Middle:
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            sb.Append('┼');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        case DataGridHorizontalBorderRendering.Bottom:
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            sb.Append('┴');
                            if (renderingHints.VerticalBorderPadding > 0)
                                sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                            break;
                        default:
                            throw new ThisShouldNeverHappenException($"Unhandled horizontal border type: {horizontalBorder}");
                    }

                    break;

                case DataGridVerticalBorderRendering.Right:

                    if (renderingHints.OuterVerticalBorders)
                    {

                        switch (horizontalBorder)
                        {
                            case DataGridHorizontalBorderRendering.None:
                                if (renderingHints.VerticalBorderPadding > 0)
                                    sb.Append(new string(' ', renderingHints.VerticalBorderPadding));
                                sb.Append('│');
                                break;
                            case DataGridHorizontalBorderRendering.Top:
                                if (renderingHints.VerticalBorderPadding > 0)
                                    sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                                sb.Append('┐');
                                break;
                            case DataGridHorizontalBorderRendering.Middle:
                                if (renderingHints.VerticalBorderPadding > 0)
                                    sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                                sb.Append('┤');
                                break;
                            case DataGridHorizontalBorderRendering.Bottom:
                                if (renderingHints.VerticalBorderPadding > 0)
                                    sb.Append(new string('─', renderingHints.VerticalBorderPadding));
                                sb.Append('┘');
                                break;
                            default:
                                throw new ThisShouldNeverHappenException($"Unhandled horizontal border type: {horizontalBorder}");
                        }
                    }

                    if (renderingHints.HighlightCurrentRow && row == Row)
                    {
                        sb.Append('<');
                    }

                    break;

                default:
                    throw new ThisShouldNeverHappenException($"Unhandled vertical border type: {verticalBorder}");
            }
        }

        public object this[string columnName]
        {
            get
            {
                FindColumnIndex(columnName, out int columnIndex);
                return CurrentRow[columnIndex];
            }
            set
            {
                FindColumnIndex(columnName, out int columnIndex);
                CurrentRow[columnIndex] = value;
            }
        }

        private DataGridColumnIndexer<object> _Object;
        private DataGridColumnIndexer<string> _String;
        private DataGridColumnIndexer<int> _Int;
        private DataGridColumnIndexer<long> _Long;
        private DataGridColumnIndexer<double> _Double;
        private DataGridColumnIndexer<decimal> _Decimal;
        private DataGridColumnIndexer<System.DateTime> _DateTime;

        public DataGridColumnIndexer<object> Object => _Object ??= new DataGridColumnIndexer<object>(this);
        public DataGridColumnIndexer<string> String => _String ??= new DataGridColumnIndexer<string>(this);
        public DataGridColumnIndexer<int> Int => _Int ??= new DataGridColumnIndexer<int>(this);
        public DataGridColumnIndexer<long> Long => _Long ??= new DataGridColumnIndexer<long>(this);
        public DataGridColumnIndexer<double> Double => _Double ??= new DataGridColumnIndexer<double>(this);
        public DataGridColumnIndexer<decimal> Decimal => _Decimal ??= new DataGridColumnIndexer<decimal>(this);
        public DataGridColumnIndexer<System.DateTime> DateTime => _DateTime ??= new DataGridColumnIndexer<System.DateTime>(this);

        private static Languages Lang { get; } = new Languages
        {
            { "Column.Index.0.{count}", "Index must be between 0 and {0}" },
            { "Column.NotFound.{name}", "Column not found: {0}" },
            { "Rows.DataMismatch.{columnType}.{dataType}", "Data type mismatch for column '{0}': expected {1}, got {2}" },
            { "Rows.NoData", "No data" },
        }
        .AddLanguages
        (
            new Language("es")
            {
                { "Column.Index.0.{count}", "El índice debe estar entre 0 y {0}" },
                { "Column.NotFound.{name}", "Columna no encontrada: {0}" },
                { "Rows.DataMismatch.{columnType}.{dataType}", "Incompatibilidad de tipo de datos para la columna '{0}': se esperaba {1}, se obtuvo {2}" },
                { "Rows.NoData", "No hay datos" },
            }
        );
    }
}
