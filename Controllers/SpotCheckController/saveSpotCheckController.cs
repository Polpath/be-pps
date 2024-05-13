using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.SpotCheckController;

[ApiController]
[Route("[controller]")]
public class saveSpotCheckController : ControllerBase
{
    public saveSpotCheckController()
    {

    }

    [HttpPost(Name = "saveSpotCheck")]
    public bool Post(SpotCheck input)
    {
        string query = "INSERT INTO [dbo].[SpotCheck]([SpotID] ,[EmployeeID] ,[CheckPointID] ,[SpotCheckDate] ,[SpotCheckImg]) VALUES ('@ID' ,'@EmpID' ,'@CheckID' , Convert(DATETIME, '@Date', 105), '@Img')";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            SqlCommand commandGetMaxID = new SqlCommand("SELECT CAST(MAX(SpotID) + 1 AS VARCHAR) AS ID FROM [dbo].[SpotCheck]", connection);
            try
            {
                connection.Open();
                SqlDataReader reader = commandGetMaxID.ExecuteReader();

                if (reader.Read())
                {
                    input.SpotCheckID = (string)reader["ID"];
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                throw;
            }

            query = query.Replace("@ID", input.SpotCheckID).Trim();
            query = query.Replace("@EmpID", input.EmployeeID).Trim();
            query = query.Replace("@CheckID", input.CheckPointID).Trim();
            query = query.Replace("@Img", input.SpotCheckImg);
            query = query.Replace("@Date", input.SpotCheckDate?.ToString());
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
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
