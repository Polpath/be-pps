using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CheckPointController
{
    [ApiController]
    [Route("[controller]")]
    public class CheckDetailController : ControllerBase
    {

        private readonly ILogger<CheckDetailController> _logger;

        public CheckDetailController(ILogger<CheckDetailController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getCheckPointDetail/{checkID}")]
        public CheckPoint Get(string checkID)
        {
            var res = new CheckPoint();

            string query = "select * from [dbo].[CheckPoint] where isActive = 1";
            string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

            if (checkID != null)
            {
                query = query + " and CheckPointID = '" + checkID + "'";
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
                        res.CheckPointID = (string)reader["CheckPointID"];
                        res.CheckPointName = (string)reader["CheckPointName"];
                        res.CheckPointDesc = (string)reader["CheckPointDesc"];
                        res.CompanyID = (string)reader["CompanyID"];
                        res.isActive = (bool)reader["isActive"];
                        res.UpdatedBy = (string)reader["UpdatedBy"];
                        res.UpdatedDate = (DateTime)reader["UpdatedDate"];
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
