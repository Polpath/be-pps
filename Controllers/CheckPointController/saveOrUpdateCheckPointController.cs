using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";
        string queryInsert = "INSERT INTO CheckPoint (CheckPointID, CheckPointName, CheckPointDesc, CompanyID , isActive, UpdatedBy ,UpdatedDate) VALUES ('@ID', '@Name', '@Desc', '@ComID', 1, 'admin', NOW())";
        string queryUpdate = "UPDATE CheckPoint SET CheckPointName = '@Name', CheckPointDesc = '@Desc', CompanyID = '@ComID', isActive = @Active, UpdatedDate = NOW() WHERE CheckPointID = '@ID'";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            if (input.CheckPointID == "0")
            {
                MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX(CAST(CheckPointID AS INT)) + 1 AS CHAR) AS ID FROM CheckPoint", connection);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = commandGetMaxID.ExecuteReader();
                    if (reader.Read())
                    {
                        var ID = ((string)reader["ID"] == null) ? "0" : reader["ID"].ToString();
                        input.CheckPointID = ID;
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

                MySqlCommand command = new MySqlCommand(queryInsert, connection);
                try
                {
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
            else
            {
                queryUpdate = queryUpdate.Replace("@ID", input.CheckPointID);
                queryUpdate = queryUpdate.Replace("@Name", input.CheckPointName);
                queryUpdate = queryUpdate.Replace("@Desc", input.CheckPointDesc);
                queryUpdate = queryUpdate.Replace("@ComID", input.CompanyID);
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
