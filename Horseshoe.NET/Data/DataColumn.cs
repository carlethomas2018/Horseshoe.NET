using System;

using Horseshoe.NET.Text;
using Horseshoe.NET.Types;

namespace Horseshoe.NET.Data
{
    /// <summary>
    /// Contains metadata and rendering information for the columns in a <see cref="DataGrid"/>.  
    /// Note, <see cref="DataColumn"/>s do not contain data.  See <see cref="DataRow"/> instead.
    /// </summary>
    public class DataColumn
    {
        private static readonly Random rand = new Random();

        private HorizontalAlign _align;
        private HorizontalAlign _hdrAlign;

        /// <summary>
        /// The name of the column.
        /// </summary>
        public string Name { get; set; }

        internal string RenderedName { get; set; }  // used internally during grid-to-text rendering

        /// <summary>
        /// The type of data stored in this column.
        /// </summary>
        public Type DataType { get; set; } = typeof(object);

        /// <summary>
        /// An optional format to use during grid rendering.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// An optional format to use for <c>null</c> values during grid rendering, default is <c>""</c>.
        /// </summary>
        public string NullFormat { get; set; }

        /// <summary>
        /// An optional horizontal alignment to use during grid rendering.
        /// </summary>
        public HorizontalAlign Align
        {
            get 
            { 
                return (_align == HorizontalAlign.None ? null : _align as HorizontalAlign?) 
                    ?? (DataType.IsNumeric() ? HorizontalAlign.Right : HorizontalAlign.Left); 
            }
            set { _align = value; }
        }

        /// <summary>
        /// An optional horizontal alignment to use for the column header during grid rendering.
        /// </summary>
        public HorizontalAlign HeaderAlign
        {
            get 
            { 
                return (_hdrAlign == HorizontalAlign.None ? null : _hdrAlign as HorizontalAlign?)
                    ?? Align; 
            }
            set { _hdrAlign = value; }
        }

        internal int CalculatedWidth { get; set; }  // used internally during grid-to-text rendering

        /// <summary>
        /// Constructs a new <see cref="DataColumn"/> instance.
        /// </summary>
        public DataColumn()
        {
            Name = "Col" + rand.Next(4096, 65535).ToString("X");  // initally assign a random name (e.g. Col1000, ColB63A, ColFFFF, etc.) to avoid duplicate and blank column names
        }

        /// <summary>
        /// Renders the details of this data column as a string, examples include...
        /// <code>
        /// { Name = 'ColB63A' }
        /// { Name = 'Employee Name', DataType = 'System.String', Width = 15 }
        /// { Name = 'Hourly Wage', DataType = 'System.Double', Format = 'C2', Align = 'Right' }
        /// </code>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{ Name = '{Name}'{(DataType == typeof(object) ? "" : $", DataType = '{DataType?.Name}'")}{(string.IsNullOrEmpty(Format) ? "" : $", Format = '{Format}'")}{(Align == HorizontalAlign.Left ? "" : $", Align = '{Align}'")}{(CalculatedWidth == 0 ? "" : $", Width = {CalculatedWidth}")} }}";
        }
    }
}
