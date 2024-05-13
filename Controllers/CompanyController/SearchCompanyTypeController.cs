using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new List<CompanyType>();

        if (companyType != null)
        {
            query = query.Replace("@param", "and CompanyTypeID = " + companyType);
        }
        else
        {
            query = query.Replace("@param", "");
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
