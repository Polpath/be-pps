using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyListController : ControllerBase
{
    public CompanyListController()
    {

    }

    [HttpGet(Name = "GetCompanyList/{empType}/{comID}")]
    public List<Company> Get(string empType, string comID)
    {
        string query = "SELECT * FROM Company where CompanyID @ID and isActive = 1";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new List<Company>();
        if (empType == "3")
        {
            query = query.Replace("@ID", "IS NOT NULL");
        }
        else
        {
            query = query.Replace("@ID", "=" + comID);
        }
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var data = new Company();
                    data.CompanyID = (string)reader["CompanyID"];
                    data.CompanyName = (string)reader["CompanyName"];
                    data.CompanyTypeID = (string)reader["CompanyTypeID"];
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
