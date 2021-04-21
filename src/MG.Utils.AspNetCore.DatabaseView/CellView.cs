namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class CellView
    {
        public CellView(string column, object cellValue)
        {
            Column = column;
            Value = cellValue?.ToString();
            ValueType = cellValue?.GetType().Name;
        }

        public string Column { get; }

        public string ValueType { get; }

        public string Value { get; }

        public override string ToString()
        {
            return $"Cell for {Column}. Value {Value}";
        }
    }
}