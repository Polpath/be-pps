using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

            if (checkID != null)
            {
                query = query + " and CheckPointID = '" + checkID + "'";
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
