using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string queryInsert = @"INSERT INTO Employee (EmployeeID ,EmployeeFirstname ,EmployeeLastname ,EmployeeCard ,EmpTypeID ,CompanyID ,isActive ,UpdatedBy ,UpdatedDate) 
                                VALUES ('@ID', '@First', '@Last', '@Card', '@Type', '@Com', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE Employee SET EmployeeFirstname = '@First', EmployeeLastname = '@Last', EmployeeCard = '@Card', EmpTypeID = '@Type', CompanyID = '@Com', isActive = @Active WHERE EmployeeID = '@ID'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (input.EmployeeID == "0")
            {
                SqlCommand commandGetMaxID = new SqlCommand("SELECT CAST(MAX(EmployeeID) + 1 AS VARCHAR) AS ID FROM Employee", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = commandGetMaxID.ExecuteReader();
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

                queryUpdate = queryUpdate.Replace("@ID", input.EmployeeID);
                queryUpdate = queryUpdate.Replace("@First", input.EmployeeFirstName);
                queryUpdate = queryUpdate.Replace("@Last", input.EmployeeLastName);
                queryUpdate = queryUpdate.Replace("@Card", input.EmployeeCard);
                queryUpdate = queryUpdate.Replace("@Type", input.EmployeeTypeID);
                queryUpdate = queryUpdate.Replace("@Com", input.CompanyID);
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
