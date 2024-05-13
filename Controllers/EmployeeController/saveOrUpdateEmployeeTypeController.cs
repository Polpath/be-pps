using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class saveOrUpdateEmployeeTypeController : ControllerBase
{

    private readonly ILogger<saveOrUpdateEmployeeTypeController> _logger;

    public saveOrUpdateEmployeeTypeController(ILogger<saveOrUpdateEmployeeTypeController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "saveOrUpdateEmployeeType")]
    public bool Post(EmployeeType input)
    {
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string queryInsert = "INSERT INTO EmployeeType (EmpTypeID, EmpTypeName , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE EmployeeType SET EmpTypeName = '@Name', isActive = @Active, UpdatedDate = GETDATE() WHERE EmpTypeID = '@ID'";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            if (input.EmployeeTypeID == "0")
            {
                MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX(EmpTypeID) + 1 AS VARCHAR) AS ID FROM EmployeeType", connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        input.EmployeeTypeID = (string)reader["ID"];
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }

                queryInsert = queryInsert.Replace("@ID", input.EmployeeTypeID);
                queryInsert = queryInsert.Replace("@Name", input.EmployeeTypeName);

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
                queryUpdate = queryUpdate.Replace("@ID", input.EmployeeTypeID);
                queryUpdate = queryUpdate.Replace("@Name", input.EmployeeTypeName);
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
