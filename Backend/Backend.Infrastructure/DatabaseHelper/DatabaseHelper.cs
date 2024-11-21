using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public interface IDatabaseHelper
{
    Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters = null);
    //execute function.

}

public class DatabaseHelper : IDatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection"); //to check it is env is production or development.
    }

    public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedureName;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
               
                    await Task.Run(() => adapter.Fill(dataTable));
                }

                return dataTable;
            }
        }
    }
}

// How to call this stored procedure.
//SqlParameter[] parameters =
//                                      {
//                                            new SqlParameter("@Id", DBNull.Value),
//                                        };
//var result = await _iDatabaseHelper.ExecuteStoredProcedureAsync("GetProductById", parameters);

// Serialize the DataTable to a JSON string
//string jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
//{
//    ContractResolver = new CamelCasePropertyNamesContractResolver()
//});