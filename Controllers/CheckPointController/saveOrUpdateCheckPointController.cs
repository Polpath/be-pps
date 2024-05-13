using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.CheckPointController;

[ApiController]
[Route("[controller]")]
public class saveOrUpdateCheckPointController : ControllerBase
{

    private readonly ILogger<saveOrUpdateCheckPointController> _logger;

    public saveOrUpdateCheckPointController(ILogger<saveOrUpdateCheckPointController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "saveOrUpdateCheck")]
    public bool Post(CheckPoint input)
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string queryInsert = "INSERT INTO [dbo].[CheckPoint] (CheckPointID, CheckPointName, CheckPointDesc, CompanyID , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', '@Desc', '@ComID', 1, 'admin', GETDATE())";
        string queryUpdate = "UPDATE [dbo].[CheckPoint] SET CheckPointName = '@Name', CheckPointDesc = '@Desc', CompanyID = '@ComID', isActive = @Active, UpdatedDate = GETDATE() WHERE CheckPointID = '@ID'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            if (input.CheckPointID == "0")
            {
                SqlCommand commandGetMaxID = new SqlCommand("SELECT CAST(MAX(CheckPointID) + 1 AS VARCHAR) AS ID FROM [dbo].[CheckPoint]", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        input.CheckPointID = (string)reader["ID"];
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                    throw;
                }

                queryInsert = queryInsert.Replace("@ID", input.CheckPointID);
                queryInsert = queryInsert.Replace("@Name", input.CheckPointName);
                queryInsert = queryInsert.Replace("@Desc", input.CheckPointDesc);
                queryInsert = queryInsert.Replace("@ComID", input.CompanyID);

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
                queryUpdate = queryUpdate.Replace("@ID", input.CheckPointID);
                queryUpdate = queryUpdate.Replace("@Name", input.CheckPointName);
                queryUpdate = queryUpdate.Replace("@Desc", input.CheckPointDesc);
                queryUpdate = queryUpdate.Replace("@ComID", input.CompanyID);
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
