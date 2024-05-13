using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyDetailController : ControllerBase
{
    public CompanyDetailController()
    {

    }

    [HttpGet(Name = "GetCompanyDetail/{companyID}")]
    public CompanyDetail Get(string companyID)
    {
        string query = "select c.CompanyName, ct.CompanyTypeID, ct.CompanyTypeName from Company c LEFT JOIN CompanyType ct on c.CompanyTypeID = ct.CompanyTypeID where CompanyID = '@ID' and c.isActive = 1";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new CompanyDetail();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            query = query.Replace("@ID", companyID);
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res.CompanyName = (string)reader["CompanyName"];
                    res.CompanyTypeID = (string)reader["CompanyTypeID"];
                    res.CompanyTypeName = (string)reader["CompanyTypeName"];
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
