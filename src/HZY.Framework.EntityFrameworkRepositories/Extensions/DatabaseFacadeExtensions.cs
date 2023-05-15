namespace HZY.Framework.EntityFrameworkRepositories.Extensions;


/// <summary>
/// DatabaseFacade 扩展
/// </summary>
public static class DatabaseFacadeExtensions
{
    /// <summary>
    /// 根据 sql 查询表格
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static DataTable QueryDataTableBySql(this DatabaseFacade database, string sql, params object[] parameters)
    {
        return QueryDataTableBySqlAsync(database, sql, parameters).Result;
    }

    /// <summary>
    /// 根据 sql 查询表格
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<DataTable> QueryDataTableBySqlAsync(this DatabaseFacade database, string sql, params object[] parameters)
    {
        var dbConnection = database.GetDbConnection();
        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (command.Connection == null) return default;
            if (command.Connection.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();
            var dt = new DataTable();

            int fieldCount = reader.FieldCount;
            for (var i = 0; i < fieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            dt.BeginLoadData();

            object[] objValues = new object[fieldCount];
            while (reader.Read())
            {
                reader.GetValues(objValues);
                dt.LoadDataRow(objValues, true);
            }

            await dbConnection.CloseAsync();

            return dt;
        }
    }

    /// <summary>
    /// 根据 sql 查询字典集合
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static List<Dictionary<string, object>> QueryDicBySql(this DatabaseFacade database, string sql, params object[] parameters)
    {
        return QueryDicBySqlAsync(database, sql, parameters).Result;
    }

    /// <summary>
    /// 根据 sql 查询字典集合
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<List<Dictionary<string, object>>> QueryDicBySqlAsync(this DatabaseFacade database, string sql, params object[] parameters)
    {
        var dbConnection = database.GetDbConnection();
        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (command.Connection == null) return default;
            if (command.Connection.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();

            var fieldCount = reader.FieldCount;
            var columns = new List<string>();
            for (int i = 0; i < fieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }

            var result = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var dic = new Dictionary<string, object>();

                foreach (var item in columns)
                {

                    var value = reader.GetValue(item);

                    dic[item] = value == DBNull.Value ? null : value;
                }

                result.Add(dic);
            }

            await dbConnection.CloseAsync();

            return result;
        }
    }

    /// <summary>
    /// 查询根据sql语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static List<T> QueryBySql<T>(this DatabaseFacade database, string sql, params object[] parameters)
        where T : class, new()
    {
        var dicts = QueryDicBySql(database, sql, parameters);

        var json = JsonConvert.SerializeObject(dicts);

        return JsonConvert.DeserializeObject<List<T>>(json);
    }

    /// <summary>
    /// 查询根据sql语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<List<T>> QueryBySqlAsync<T>(this DatabaseFacade database, string sql, params object[] parameters)
        where T : class, new()
    {
        var dicts = await QueryDicBySqlAsync(database, sql, parameters);

        var json = JsonConvert.SerializeObject(dicts);

        return JsonConvert.DeserializeObject<List<T>>(json);
    }

    /// <summary>
    /// 查询根据sql返回单个值
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static object QuerySingleBySql(this DatabaseFacade database, string sql, params object[] parameters)
    {
        return QuerySingleBySql<object>(database, sql, parameters);
    }

    /// <summary>
    /// 查询根据sql返回单个值
    /// </summary>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<object> QuerySingleBySqlAsync(this DatabaseFacade database, string sql, params object[] parameters)
    {
        return await QuerySingleBySqlAsync<object>(database, sql, parameters);
    }

    /// <summary>
    /// 查询根据sql返回单个值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static T QuerySingleBySql<T>(this DatabaseFacade database, string sql, params object[] parameters)
    {
        return QuerySingleBySqlAsync<T>(database, sql, parameters).Result;
    }

    /// <summary>
    /// 查询根据sql返回单个值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<T> QuerySingleBySqlAsync<T>(this DatabaseFacade database, string sql, params object[] parameters)
    {
        var dbConnection = database.GetDbConnection();
        using (var command = dbConnection.CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (command.Connection == null) return default;
            if (command.Connection.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            command.Parameters.AddRange(parameters);

            var result = await command.ExecuteScalarAsync();

            await dbConnection.CloseAsync();

            if (result == null)
            {
                return default;
            }

            return (T)result;

        }
    }


}
