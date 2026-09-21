namespace Horseshoe.NET.Data
{
    public class DataGridColumnIndexer<T>
    {
        readonly DataGrid parent;

        public DataGridColumnIndexer(DataGrid parent)
        {
            this.parent = parent;
        }

        public T this[string columnName]
        {
            get
            {
                parent.FindColumnIndex(columnName, out int columnIndex);
                return (T)parent.CurrentRow[columnIndex];
            }
            set
            {
                parent.FindColumnIndex(columnName, out int columnIndex);
                parent.CurrentRow[columnIndex] = value;
            }
        }
    }
}
