using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class TableView
    {
        public TableView(IEntityType entityType, Exception exception)
            : this(entityType, Array.Empty<RowView>())
        {
            RaisedErrorOrNull = exception;
        }

        public TableView(IEntityType entityType, IReadOnlyCollection<RowView> rows)
        {
            Rows = rows;
            Name = entityType.GetTableName();
            TableSchema = entityType.GetSchema();
            Columns = entityType.GetProperties().Select(x => x.Name).ToArray();

            if (rows.Any())
            {
                string[] unexpectedColumns = Columns.Except(rows.First().Columns).ToArray();

                if (unexpectedColumns.Any())
                {
                    var messagePart = unexpectedColumns.Aggregate(
                        string.Empty,
                        (seed, curr) => seed + curr + ", ");

                    throw new InvalidOperationException($"Some unexpected Columns appeared: " + messagePart);
                }
            }
        }

        public IReadOnlyCollection<string> Columns { get; }

        public IReadOnlyCollection<RowView> Rows { get; }

        public string Name { get; }

        public string TableSchema { get; }

        public Exception RaisedErrorOrNull { get; }

        public bool Successful => RaisedErrorOrNull == null;

        public override string ToString()
        {
            return $"{Name}: Columns {Columns.Count}, Rows {Rows.Count}";
        }
    }
}