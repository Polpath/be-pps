using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

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

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
