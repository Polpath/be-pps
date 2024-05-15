using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        var res = new Employee();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            query = query.Replace("@empID", empID);
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res.EmployeeID = (string)reader["EmployeeID"];
                    res.EmployeeFirstName = (string)reader["EmployeeFirstName"];
                    res.EmployeeLastName = (string)reader["EmployeeLastName"];
                    res.EmployeeCard = (string)reader["EmployeeCard"];
                    res.EmployeeTypeID = (string)reader["EmpTypeID"];
                    res.CompanyID = (string)reader["CompanyID"];
                    res.isActive = ((ulong)reader["isActive"] == 1) ? true : false;
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