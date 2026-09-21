namespace Horseshoe.NET.Data
{
    public class DataRowColumnIndexer<T>
    {
        readonly DataRow parent;

        public DataRowColumnIndexer(DataRow parent)
        {
            this.parent = parent;
        }

        public T this[string columnName]
        {
            get
            {
                parent.Parent.FindColumnIndex(columnName, out int columnIndex);
                return (T)parent[columnIndex];
            }
            set
            {
                parent.Parent.FindColumnIndex(columnName, out int columnIndex);
                parent[columnIndex] = value;
            }
        }
    }
}
