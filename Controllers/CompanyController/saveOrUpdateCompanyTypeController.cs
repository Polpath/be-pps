using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class saveOrUpdateCompanyTypeController : ControllerBase
{

    private readonly ILogger<saveOrUpdateCompanyTypeController> _logger;

    public saveOrUpdateCompanyTypeController(ILogger<saveOrUpdateCompanyTypeController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "saveOrUpdateCompanyType")]
    public bool Post(CompanyType input)
    {
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string queryInsert = "INSERT INTO CompanyType (CompanyTypeID, CompanyTypeName , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', 1, 'admin', NOW())";
        string queryUpdate = "UPDATE CompanyType SET CompanyTypeName = '@Name', isActive = @Active, UpdatedDate = NOW() WHERE CompanyTypeID = '@ID'";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            if (input.CompanyTypeID == "0")
            {
                MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX((CAST(CompanyTypeID AS INT)) + 1 AS CHAR) AS ID FROM CompanyType", connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        input.CompanyTypeID = (string)reader["ID"];
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }

                queryInsert = queryInsert.Replace("@ID", input.CompanyTypeID);
                queryInsert = queryInsert.Replace("@Name", input.CompanyTypeName);

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
                queryUpdate = queryUpdate.Replace("@ID", input.CompanyTypeID);
                queryUpdate = queryUpdate.Replace("@Name", input.CompanyTypeName);
                queryUpdate = queryUpdate.Replace("@Active", input?.isActive == true ? "1" : "0");

                MySqlCommand command = new MySqlCommand(queryUpdate, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    connection.Close();
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
