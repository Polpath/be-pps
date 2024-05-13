using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class saveOrUpdateEmployeeController : ControllerBase
{

    private readonly ILogger<saveOrUpdateEmployeeController> _logger;

    public saveOrUpdateEmployeeController(ILogger<saveOrUpdateEmployeeController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "saveOrUpdateEmployee")]
    public bool Post(Employee input)
    {
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string queryInsert = @"INSERT INTO Employee (EmployeeID ,EmployeeFirstname ,EmployeeLastname ,EmployeeCard ,EmpTypeID ,CompanyID ,isActive ,UpdatedBy ,UpdatedDate) 
                                VALUES ('@ID', '@First', '@Last', '@Card', '@Type', '@Com', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE Employee SET EmployeeFirstname = '@First', EmployeeLastname = '@Last', EmployeeCard = '@Card', EmpTypeID = '@Type', CompanyID = '@Com', isActive = @Active WHERE EmployeeID = '@ID'";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            if (input.EmployeeID == "0")
            {
                MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX(EmployeeID) + 1 AS VARCHAR) AS ID FROM Employee", connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        input.EmployeeID = (string)reader["ID"];
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }

                queryInsert = queryInsert.Replace("@ID", input.EmployeeID);
                queryInsert = queryInsert.Replace("@First", input.EmployeeFirstName);
                queryInsert = queryInsert.Replace("@Last", input.EmployeeLastName);
                queryInsert = queryInsert.Replace("@Card", input.EmployeeCard);
                queryInsert = queryInsert.Replace("@Type", input.EmployeeTypeID);
                queryInsert = queryInsert.Replace("@Com", input.CompanyID);

                MySqlCommand command = new MySqlCommand(queryInsert, connection);
                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }
            }
            else
            {

                queryUpdate = queryUpdate.Replace("@ID", input.EmployeeID);
                queryUpdate = queryUpdate.Replace("@First", input.EmployeeFirstName);
                queryUpdate = queryUpdate.Replace("@Last", input.EmployeeLastName);
                queryUpdate = queryUpdate.Replace("@Card", input.EmployeeCard);
                queryUpdate = queryUpdate.Replace("@Type", input.EmployeeTypeID);
                queryUpdate = queryUpdate.Replace("@Com", input.CompanyID);
                queryUpdate = queryUpdate.Replace("@Active", input?.isActive == true ? "1" : "0");

                MySqlCommand command = new MySqlCommand(queryUpdate, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
