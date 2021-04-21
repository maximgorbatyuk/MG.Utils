using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class RowView
    {
        public RowView(IReadOnlyCollection<CellView> cells)
        {
            Cells = cells;
            Columns = cells.Select(x => x.Column).ToArray();
        }

        public IReadOnlyCollection<CellView> Cells { get; }

        public IReadOnlyCollection<string> Columns { get; }

        public string CellValueByColumnName(string columnName)
        {
            return (Cells.FirstOrDefault(x => x.Column == columnName)
                    ?? throw new InvalidOperationException($"Could not find Cell by name {columnName}"))
                .Value;
        }
    }
}