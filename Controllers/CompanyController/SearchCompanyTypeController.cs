using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class SearchCompanyTypeController : ControllerBase
{

    private readonly ILogger<SearchCompanyTypeController> _logger;

    public SearchCompanyTypeController(ILogger<SearchCompanyTypeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getSearchCompanyType/{companyType}")]
    public List<CompanyType> Get(string? companyType)
    {
        string query = "SELECT * FROM CompanyType where isActive = 1 @param";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new List<CompanyType>();

        if (companyType != null)
        {
            query = query.Replace("@param", "and CompanyTypeID = " + companyType);
        }
        else
        {
            query = query.Replace("@param", "");
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
