using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BCSH2_Sem_prace_Chyska.Controllers
{
    public class DatabaseHelpers
    {

        public static IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

       public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ProjectDatabase");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int rowsAffectedCount = command.ExecuteNonQuery();
                        return rowsAffectedCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                return -1; // An error occurred
            }
        }
        public static IDataReader OpenReader(string query, params SqlParameter[] parameters)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ProjectDatabase");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        return command.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
