using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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

            string query = "select * from CheckPoint where isActive = 1";
            string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

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

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var data = new CheckPoint();
                        data.CheckPointID = (string)reader["CheckPointID"];
                        data.CheckPointName = (string)reader["CheckPointName"];
                        data.CheckPointDesc = (string)reader["CheckPointDesc"];
                        data.CompanyID = comData.FirstOrDefault(src => src.CompanyID == (string)reader["CompanyID"])?.CompanyName;
                        data.isActive = ((ulong)reader["isActive"] == 1) ? true : false;
                        data.UpdatedBy = (string)reader["UpdatedBy"];
                        data.UpdatedDate = (DateTime)reader["UpdatedDate"];
                        res.Add(data);
                    }
                    reader.Close();
                    connection.Close();
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
