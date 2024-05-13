using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class SearchEmployeeController : ControllerBase
{

    private readonly ILogger<SearchEmployeeController> _logger;

    public SearchEmployeeController(ILogger<SearchEmployeeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "SearchEmployee/{empName}/{companyID}/{empType}")]
    public List<Employee> Get(string? empName, string? companyID, string? empType)
    {
        string query = "SELECT * FROM Employee where isActive = 1";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new List<Employee>();

        if (empName != null)
        {
            query = query + " and concat(EmployeeFirstname, ' ', EmployeeLastname) like '%" + empName + "%'";
        }
        if (companyID != null)
        {
            query = query + " and CompanyID = " + companyID;
        }
        if (empType != null)
        {
            query = query + " and EmpTypeID = " + empType;
        }

        EmployeeTypeListController EmpTypeService = new EmployeeTypeListController();
        var empTypeData = EmpTypeService.Get("3");

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
                    var data = new Employee();
                    data.EmployeeID = (string)reader["EmployeeID"];
                    data.EmployeeFirstName = (string)reader["EmployeeFirstName"];
                    data.EmployeeLastName = (string)reader["EmployeeLastName"];
                    data.EmployeeCard = (string)reader["EmployeeCard"];
                    data.CompanyID = comData.FirstOrDefault(src => src.CompanyID == (string)reader["CompanyID"])?.CompanyName;
                    data.EmployeeTypeID = empTypeData.FirstOrDefault(src => src.EmployeeTypeID == (string)reader["EmpTypeID"])?.EmployeeTypeName;
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
