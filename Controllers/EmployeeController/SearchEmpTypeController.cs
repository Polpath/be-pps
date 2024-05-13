using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new List<EmployeeType>();

        if (empType != null)
        {
            query = query.Replace("@param", "and EmpTypeID = " + empType);
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
                    var data = new EmployeeType();
                    data.EmployeeTypeID = (string)reader["EmpTypeID"];
                    data.EmployeeTypeName = (string)reader["EmpTypeName"];
                    data.isActive = ((ulong)reader["isActive"] == 1) ? true : false;
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
