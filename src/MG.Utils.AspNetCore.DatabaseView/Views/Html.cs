using System.Collections.Generic;
using System.Linq;

namespace MG.Utils.AspNetCore.DatabaseView.Views
{
    internal class Html
    {
        private readonly IReadOnlyCollection<TableView> _tables;

        public Html(IReadOnlyCollection<TableView> tables)
        {
            _tables = tables;
        }

        public override string ToString()
        {
            return Header() + TablesContext() + Footer();
        }

        public static implicit operator string(Html html) => html.ToString();

        private string TablesContext()
        {
            var seed = string.Empty;

            foreach (TableView table in _tables)
            {
                seed +=
                    @$"<div class=""mt-3"">
<hr />
<div>
    <div class=""h3"">{table.Name}</div>
    <div>Count: {table.Rows.Count}</div>
</div>
<div class=""table-responsive"">
    <table class=""table table-striped table-hover table-sm table-bordered"">
        <thead>
        <tr>
            {table.Columns.Aggregate(string.Empty, (curr, next) => curr + $"<th scope=\"col\">{next}</th>")}
        </tr>
        </thead>
        <tbody>";

                if (table.Successful)
                {
                    foreach (RowView row in table.Rows)
                    {
                        seed +=
                            $@"<tr>{table.Columns.Aggregate(string.Empty, (curr, next) => curr + $"<td scope=\"col\">{row.CellValueByColumnName(next)}</td>")}</tr>";
                    }

                    seed += "</tbody></table>";
                }
                else
                {
                    seed +=
                        @$"<div class=""mt-2"">
    <div>The query has failed</div>
    <div>
    <div>{table.RaisedErrorOrNull.Message}</div>
    <div>{table.RaisedErrorOrNull.InnerException?.Message}</div>
    </div>
</div>";
                }

                seed += "</div></div>";
            }

            return seed;
        }

        private string Header()
        {
            return
 @"<!doctype html>
<html lang=""en"">
<head>
    <!-- Required meta tags -->
    <meta charset=""utf -8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css""
          rel=""stylesheet"" integrity=""sha384-eOJMYsd53ii+scO/bJGFsiCZc+5NDVN2yr8+0RDqr0Ql0h+rP48ckxlpbzKgwra6""
          crossorigin=""anonymous"">

    <title>Tables</title>
</head>
<body>

    <div class=""container-fluid"">
        <h1 class=""mb-3"">Database tables</h1>";
        }

        private string Footer()
        {
            return @"</div>
<script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.bundle.min.js""
        integrity=""sha384-JEW9xMcG8R+pH31jmWH6WWP0WintQrMb4s7ZOdauHnUtxwoG2vI5DkLtS3qm9Ekf""
        crossorigin=""anonymous""></script>
</body>
</html>";
        }
    }
}