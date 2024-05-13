using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.LoginController;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Login")]
    public Auth Post(Login input)
    {
        var res = new Auth();

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        var base64EncodedBytes = Convert.FromBase64String(input.Password);
        var pass64 = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        string query = "SELECT * FROM Employee where EmployeeFirstname = @Username and EmployeeCard = @Password and isActive = 1";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", input.Username);
            command.Parameters.AddWithValue("@Password", pass64);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        res.EmployeeID = (string)reader["EmployeeID"];
                        res.EmployeeName = (string)reader["EmployeeFirstName"] + " " + (string)reader["EmployeeLastName"];
                        res.EmployeeCard = (string)reader["EmployeeCard"];
                        res.EmpTypeID = (string)reader["EmpTypeID"];
                        res.CompanyID = (string)reader["CompanyID"];
                        res.Status = true;
                    }
                }
                else
                {
                    res.Status = false;
                }
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
