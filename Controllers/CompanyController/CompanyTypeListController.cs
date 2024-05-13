using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyTypeListController : ControllerBase
{
    public CompanyTypeListController()
    {

    }

    [HttpGet(Name = "GetCompanyTypeList/{empType}/{comType}")]
    public List<CompanyType> Get(string empType, string comType)
    {
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string query = "SELECT * FROM CompanyType where CompanyTypeID = @ComType and isActive = 1";

        var res = new List<CompanyType>();

        if (empType == "3")
        {
            query = query.Replace("= @ComType", "IS NOT NULL");
        }
        else
        {
            query = query.Replace("@ComType", comType);
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
                    var data = new CompanyType();
                    data.CompanyTypeID = (string)reader["CompanyTypeID"];
                    data.CompanyTypeName = (string)reader["CompanyTypeName"];
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
