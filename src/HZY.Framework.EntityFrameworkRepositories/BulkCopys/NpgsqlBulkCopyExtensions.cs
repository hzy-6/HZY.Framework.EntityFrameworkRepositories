namespace HZY.Framework.EntityFrameworkRepositories.Extensions;

/// <summary>
/// Npgsql
/// </summary>
public static class NpgsqlBulkCopyExtensions
{

    /// <summary>
    /// Npgsql 批量拷贝数据
    /// </summary>
    /// <param name="database"></param>
    /// <param name="dataTable"></param>
    /// <param name="tableName"></param>
    /// <param name="ignoreColumns"></param>
    /// <exception cref="Exception"></exception>
    public static void NpgsqlBulkCopy(this DatabaseFacade database, DataTable dataTable, string tableName, params string[] ignoreColumns)
    {
        if (!database.IsNpgsql())
        {
            throw new Exception("当前不是 Npgsql 数据库无法调用此函数!");
        }
        var dbConnection = (NpgsqlConnection)database.GetDbConnection();

        if (ignoreColumns != null || ignoreColumns.Length > 0)
        {
            //忽略某列
            foreach (var item in ignoreColumns)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                if (!dataTable.Columns.Contains(item)) continue;
                dataTable.Columns.Remove(item);
            }
        }

        var fields = new List<string>();
        foreach (DataColumn item in dataTable.Columns)
        {
            fields.Add($"\"{item.ColumnName}\"");
        }

        try
        {
            // 构建导入SQL
            var commandFormat = string.Format("COPY \"{0}\"({1}) FROM STDIN BINARY", tableName, string.Join(",", fields));

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            using (var writer = dbConnection.BeginBinaryImport(commandFormat))
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    writer.WriteRow(item.ItemArray);
                }

                writer.Complete();
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Npgsql 批量拷贝数据
    /// </summary>
    /// <param name="database"></param>
    /// <param name="dataTable"></param>
    /// <param name="tableName"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="ignoreColumns"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task NpgsqlBulkCopyAsync(this DatabaseFacade database, DataTable dataTable, string tableName, CancellationToken cancellationToken = default, params string[] ignoreColumns)
    {
        if (!database.IsNpgsql())
        {
            throw new Exception("当前不是 Npgsql 数据库无法调用此函数!");
        }
        var dbConnection = (NpgsqlConnection)database.GetDbConnection();

        if (ignoreColumns != null || ignoreColumns.Length > 0)
        {
            //忽略某列
            foreach (var item in ignoreColumns)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                if (!dataTable.Columns.Contains(item)) continue;
                dataTable.Columns.Remove(item);
            }
        }

        var fields = new List<string>();
        foreach (DataColumn item in dataTable.Columns)
        {
            fields.Add($"\"{item.ColumnName}\"");
        }

        try
        {
            // 构建导入SQL
            var commandFormat = string.Format("COPY \"{0}\"({1}) FROM STDIN BINARY", tableName, string.Join(",", fields));

            if (dbConnection.State != ConnectionState.Open)
            {
                await dbConnection.OpenAsync();
            }

            using (var writer = dbConnection.BeginBinaryImport(commandFormat))
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    await writer.WriteRowAsync(cancellationToken, item.ItemArray);
                }

                await writer.CompleteAsync(cancellationToken);
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                await dbConnection.CloseAsync();
            }
        }
    }

    /// <summary>
    /// Npgsql 批量拷贝数据
    /// </summary>
    /// <param name="database"></param>
    /// <param name="items"></param>
    /// <param name="tableName"></param>
    /// <param name="ignoreColumns"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static void NpgsqlBulkCopy<T>(this DatabaseFacade database, List<T> items, string tableName, params string[] ignoreColumns)
        where T : class, new()
    {
        var dataTable = items.ToDataTable();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var tableAttribute = type.GetTableAttribute();
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
        }

        database.NpgsqlBulkCopy(dataTable, tableName, ignoreColumns);
    }

    /// <summary>
    /// Npgsql 批量拷贝数据
    /// </summary>
    /// <param name="database"></param>
    /// <param name="items"></param>
    /// <param name="tableName"></param>
    /// <param name="ignoreColumns"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task NpgsqlBulkCopyAsync<T>(this DatabaseFacade database, List<T> items, string tableName, params string[] ignoreColumns)
        where T : class, new()
    {
        var dataTable = items.ToDataTable();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var tableAttribute = type.GetTableAttribute();
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
        }

        return database.NpgsqlBulkCopyAsync(dataTable, tableName, default, ignoreColumns);
    }
}
