using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class SearchEmpTypeController : ControllerBase
{

    private readonly ILogger<SearchEmpTypeController> _logger;

    public SearchEmpTypeController(ILogger<SearchEmpTypeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getSearchEmployeeType/{empType}")]
    public List<EmployeeType> Get(string? empType)
    {
        string query = "SELECT * FROM EmployeeType where isActive = 1 @param";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new List<EmployeeType>();

        if (empType != null)
        {
            query = query.Replace("@param", "and EmpTypeID = " + empType);
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
                    var data = new EmployeeType();
                    data.EmployeeTypeID = (string)reader["EmpTypeID"];
                    data.EmployeeTypeName = (string)reader["EmpTypeName"];
                    data.isActive = (bool)reader["isActive"];
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
