using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.SpotCheckController;

[ApiController]
[Route("[controller]")]
public class SpotListController : ControllerBase
{
    public SpotListController()
    {

    }

    [HttpGet(Name = "GetSpotCheckList/{comID}/{spotForm}/{spotTo}")]
    public List<SpotCheck> Get(string? comID, string spotForm, string spotTo)
    {
        string query = "select * from SpotCheck s LEFT JOIN [CheckPoint] c on s.CheckPointID = c.CheckPointID where CAST(FORMAT(s.SpotCheckDate, 'yyyy-MM-dd') as DATE) >= '@Form' and CAST(FORMAT(s.SpotCheckDate, 'yyyy-MM-dd') as DATE) <= '@TO' @ComID";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new List<SpotCheck>();
        if (comID == null)
        {
            query = query.Replace("@ComID", "");
        }
        else
        {
            query = query.Replace("@ComID", "and c.CompanyID = " + comID);
        }

        query = query.Replace("@Form", spotForm);
        query = query.Replace("@TO", spotTo);


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
                    var data = new SpotCheck();
                    data.SpotCheckID = (string)reader["SpotID"];
                    data.SpotCheckDate = ((DateTime)reader["SpotCheckDate"]).ToString("dd-MM-yyyy");
                    data.CompanyName = comData.FirstOrDefault(src => src.CompanyID == (string)reader["CompanyID"])?.CompanyName;
                    data.CheckPointName = (string)reader["CheckPointName"];
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
