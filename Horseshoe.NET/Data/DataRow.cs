using System.Collections.Generic;

namespace Horseshoe.NET.Data
{
    public class DataRow : List<object>
    {
        public DataGrid Parent { get; set; }

        public object this[string columnName]
        {
            get
            {
                Parent.FindColumnIndex(columnName, out int columnIndex);
                return this[columnIndex];
            }
            set
            {
                Parent.FindColumnIndex(columnName, out int columnIndex);
                this[columnIndex] = value;
            }
        }

        public DataRow() 
        { 
        }

        public DataRow(IEnumerable<object> values) : base(values)
        {
        }

        //private DataRowColumnIndexer<object> _Object;
        private DataRowColumnIndexer<string> _String;
        private DataRowColumnIndexer<int> _Int;
        private DataRowColumnIndexer<long> _Long;
        private DataRowColumnIndexer<double> _Double;
        private DataRowColumnIndexer<decimal> _Decimal;
        private DataRowColumnIndexer<System.DateTime> _DateTime;

        //public DataRowColumnIndexer<object> Object => _Object ??= new DataRowColumnIndexer<object>(this);
        public DataRowColumnIndexer<string> String => _String ??= new DataRowColumnIndexer<string>(this);
        public DataRowColumnIndexer<int> Int => _Int ??= new DataRowColumnIndexer<int>(this);
        public DataRowColumnIndexer<long> Long => _Long ??= new DataRowColumnIndexer<long>(this);
        public DataRowColumnIndexer<double> Double => _Double ??= new DataRowColumnIndexer<double>(this);
        public DataRowColumnIndexer<decimal> Decimal => _Decimal ??= new DataRowColumnIndexer<decimal>(this);
        public DataRowColumnIndexer<System.DateTime> DateTime => _DateTime ??= new DataRowColumnIndexer<System.DateTime>(this);
    }
}
