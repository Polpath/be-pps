using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class SearchCompanyController : ControllerBase
{

    private readonly ILogger<SearchCompanyController> _logger;

    public SearchCompanyController(ILogger<SearchCompanyController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getSearchCompany/{companyID}/{companyType}")]
    public List<Company> Get(string? companyID, string? companyType)
    {
        string query = "SELECT * FROM Company where isActive = 1 @param";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new List<Company>();

        if (companyID != null && companyType != null)
        {
            query = query.Replace("@param", "and CompanyID = " + companyID + " and CompanyTypeID = " + companyType);
        }
        else if (companyID == null && companyType != null)
        {
            query = query.Replace("@param", " and CompanyTypeID = " + companyType);
        }
        else if (companyID != null && companyType == null)
        {
            query = query.Replace("@param", "and CompanyID = " + companyID);
        }
        else
        {
            query = query.Replace("@param", "");
        }

        CompanyTypeListController comTypeService = new CompanyTypeListController();
        var comTypeData = comTypeService.Get("3", "1");

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
                    data.CompanyTypeID = comTypeData?.FirstOrDefault(src => src.CompanyTypeID == (string)reader["CompanyTypeID"])?.CompanyTypeName;
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
