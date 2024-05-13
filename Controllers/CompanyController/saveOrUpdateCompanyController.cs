using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class saveOrUpdateCompanyController : ControllerBase
{

    private readonly ILogger<saveOrUpdateCompanyController> _logger;

    public saveOrUpdateCompanyController(ILogger<saveOrUpdateCompanyController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "saveOrUpdateCompany")]
    public bool Post(Company input)
    {
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string queryInsert = "INSERT INTO Company (CompanyID, CompanyName ,CompanyTypeID , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', '@TypeID', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE Company SET CompanyName = '@Name', CompanyTypeID = '@TypeID', isActive = @Active, UpdatedDate = GETDATE() WHERE CompanyID = '@ID'";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            if (input.CompanyID == "0")
            {
                MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX(CompanyID) + 1 AS VARCHAR) AS ID FROM Company", connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        input.CompanyID = (string)reader["ID"];
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }

                queryInsert = queryInsert.Replace("@ID", input.CompanyID);
                queryInsert = queryInsert.Replace("@Name", input.CompanyName);
                queryInsert = queryInsert.Replace("@TypeID", input.CompanyTypeID);

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
                queryUpdate = queryUpdate.Replace("@ID", input.CompanyID);
                queryUpdate = queryUpdate.Replace("@Name", input.CompanyName);
                queryUpdate = queryUpdate.Replace("@TypeID", input.CompanyTypeID);
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
