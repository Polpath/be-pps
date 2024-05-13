using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
            string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

            if (spotID != null)
            {
                query = query + " where SpotID = '" + spotID + "'";
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
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
