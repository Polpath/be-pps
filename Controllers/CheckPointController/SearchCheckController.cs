using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.CheckPointController
{
    [ApiController]
    [Route("[controller]")]
    public class SearchCheckController : ControllerBase
    {

        private readonly ILogger<SearchCheckController> _logger;

        public SearchCheckController(ILogger<SearchCheckController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "searchCheckPoint/{checkName}/{companyID}")]
        public List<CheckPoint> Get(string? checkName, string? companyID)
        {
            var res = new List<CheckPoint>();

            string query = "select * from [dbo].[CheckPoint] where isActive = 1";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

            if (checkName != null)
            {
                query = query + " and CheckPointName like '%" + checkName + "%'";
            }
            if (companyID != null)
            {
                query = query + " and CompanyID = '" + companyID + "'";
            }

            CompanyListController comService = new CompanyListController();
            var comData = comService.Get("3", "1");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var data = new CheckPoint();
                        data.CheckPointID = (string)reader["CheckPointID"];
                        data.CheckPointName = (string)reader["CheckPointName"];
                        data.CheckPointDesc = (string)reader["CheckPointDesc"];
                        data.CompanyID = comData.FirstOrDefault(src => src.CompanyID == (string)reader["CompanyID"])?.CompanyName;
                        data.isActive = (bool)reader["isActive"];
                        data.UpdatedBy = (string)reader["UpdatedBy"];
                        data.UpdatedDate = (DateTime)reader["UpdatedDate"];
                        res.Add(data);
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
