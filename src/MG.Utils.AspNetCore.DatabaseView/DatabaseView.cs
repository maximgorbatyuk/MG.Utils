using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MG.Utils.AspNetCore.DatabaseView
{
    internal class DatabaseView<TDatabase>
        where TDatabase : DbContext
    {
        private readonly TDatabase _context;
        private readonly string _tableNameOrNull;

        public DatabaseView(TDatabase context, string tableNameOrNull = null)
        {
            _context = context;
            _tableNameOrNull = tableNameOrNull;
        }

        public async Task<IReadOnlyCollection<TableView>> TablesAsync()
        {
            var tableViews = new List<TableView>();

            // https://stackoverflow.com/a/57014475
            // We need the class reference to construct the method call
            Type thisClassReference = GetType();

            // We need the name of generic method to call using the class reference
            MethodInfo mi = thisClassReference.GetMethod(
                nameof(QueryAsync), BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Could not find Method {nameof(QueryAsync)}");

            foreach (IEntityType entityType in GetDatabaseTypes())
            {
                TableView table;
                try
                {
                    // This creates a callable MethodInfo with our generic type
                    MethodInfo miConstructed = mi.MakeGenericMethod(entityType.ClrType);

                    // This calls the method with the generic type using Invoke
                    var rowsTask = (Task<IReadOnlyCollection<RowView>>)miConstructed.Invoke(this, null);

                    // https://stackoverflow.com/a/16153317
                    IReadOnlyCollection<RowView> rows = await rowsTask;
                    table = new TableView(entityType, rows);
                }
                catch (Exception e)
                {
                    table = new TableView(
                        entityType,
                        new InvalidOperationException(
                            $"Could not build database view for {entityType.Name}", e));
                }

                tableViews.Add(table);
            }

            return tableViews;
        }

        private IEnumerable<IEntityType> GetDatabaseTypes()
        {
            // https://stackoverflow.com/a/54188114
            IEnumerable<IEntityType> types = _context.Model.GetEntityTypes();

            return _tableNameOrNull != null ? types.Where(x => x.ClrType.Name == _tableNameOrNull) : types;
        }

        private async Task<IReadOnlyCollection<RowView>> QueryAsync<T>()
            where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            try
            {
                return (await _context.Set<T>()
                        .AsNoTracking()
                        .ToArrayAsync())
                    .Select(x =>
                    {
                        var cells = new List<CellView>();
                        foreach (PropertyInfo propertyInfo in properties)
                        {
                            cells.Add(new CellView(propertyInfo.Name, propertyInfo.GetValue(x, null)));
                        }

                        return cells;
                    })
                    .Select(x => new RowView(x))
                    .ToArray();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Could not load rows for table {typeof(T).Name}", e);
            }
        }
    }
}