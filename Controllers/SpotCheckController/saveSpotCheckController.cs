using be.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Xml;

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
        string query = "INSERT INTO SpotCheck (SpotID ,EmployeeID ,CheckPointID ,SpotCheckDate ,SpotCheckImg) VALUES ('@ID' ,'@EmpID' ,'@CheckID' , STR_TO_DATE('@Date','%Y-%m-%dT%H:%i:%s.%f'), '@Img')";
        string connectionString = @"server=b3tii4asmutgre5gyouk-mysql.services.clever-cloud.com;user=u2zqys3tn1mblv7m;database=b3tii4asmutgre5gyouk;port=3306;password=G6XH5FBjQWIES1QIuW9M";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {

            MySqlCommand commandGetMaxID = new MySqlCommand("SELECT CAST(MAX(SpotID) + 1 AS CHAR) AS ID FROM SpotCheck", connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = commandGetMaxID.ExecuteReader();

                if (reader.Read())
                {
                    input.SpotCheckID = (reader["ID"] == DBNull.Value) ? "1" : (string)reader["ID"];
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
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
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
