using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string queryInsert = "INSERT INTO CompanyType (CompanyTypeID, CompanyTypeName , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE CompanyType SET CompanyTypeName = '@Name', isActive = @Active, UpdatedDate = GETDATE() WHERE CompanyTypeID = '@ID'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (input.CompanyTypeID == "0")
            {
                SqlCommand commandGetMaxID = new SqlCommand("SELECT CAST(MAX(CompanyTypeID) + 1 AS VARCHAR) AS ID FROM CompanyType", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = commandGetMaxID.ExecuteReader();
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
                queryUpdate = queryUpdate.Replace("@ID", input.CompanyTypeID);
                queryUpdate = queryUpdate.Replace("@Name", input.CompanyTypeName);
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
