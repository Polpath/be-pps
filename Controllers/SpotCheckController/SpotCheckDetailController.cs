using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.SpotCheckController
{
    [ApiController]
    [Route("[controller]")]
    public class SpotCheckDetailController : ControllerBase
    {

        private readonly ILogger<SpotCheckDetailController> _logger;

        public SpotCheckDetailController(ILogger<SpotCheckDetailController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getSpotCheckDetail/{spotID}")]
        public SpotCheck Get(string spotID)
        {
            var res = new SpotCheck();

            string query = "select * from [dbo].[SpotCheck]";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

            if (spotID != null)
            {
                query = query + " where SpotID = '" + spotID + "'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res.SpotCheckID = (string)reader["SpotID"].ToString().Trim();
                        res.EmployeeID = (string)reader["EmployeeID"].ToString().Trim();
                        res.CheckPointID = (string)reader["CheckPointID"].ToString().Trim();

                        string input = ((DateTime)reader["SpotCheckDate"]).ToString();
                        DateTime dateTime = DateTime.Parse(input);
                        res.SpotCheckDate = dateTime.ToString("dd-MM-yyyy HH:mm:ss");

                        res.SpotCheckImg = (string)reader["SpotCheckImg"];
                    }
                    reader.Close();
                    return res;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
