using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string queryInsert = "INSERT INTO EmployeeType (EmpTypeID, EmpTypeName , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE EmployeeType SET EmpTypeName = '@Name', isActive = @Active, UpdatedDate = GETDATE() WHERE EmpTypeID = '@ID'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (input.EmployeeTypeID == "0")
            {
                SqlCommand commandGetMaxID = new SqlCommand("SELECT CAST(MAX(EmpTypeID) + 1 AS VARCHAR) AS ID FROM EmployeeType", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = commandGetMaxID.ExecuteReader();
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

                SqlCommand command = new SqlCommand(queryInsert, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
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

                SqlCommand command = new SqlCommand(queryUpdate, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
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
