using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class EmployeeDetailController : ControllerBase
{
    public EmployeeDetailController()
    {

    }

    [HttpGet(Name = "GetEmployeeDetail/{empID}")]
    public Employee Get(string empID)
    {
        string query = "select * from Employee where EmployeeID = '@empID' and isActive = 1";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new Employee();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            query = query.Replace("@empID", empID);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res.EmployeeID = (string)reader["EmployeeID"];
                    res.EmployeeFirstName = (string)reader["EmployeeFirstName"];
                    res.EmployeeLastName = (string)reader["EmployeeLastName"];
                    res.EmployeeCard = (string)reader["EmployeeCard"];
                    res.EmployeeTypeID = (string)reader["EmpTypeID"];
                    res.CompanyID = (string)reader["CompanyID"];
                    res.isActive = (bool)reader["isActive"];
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