using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyListController : ControllerBase
{
    public CompanyListController()
    {

    }

    [HttpGet(Name = "GetCompanyList/{empType}/{comID}")]
    public List<Company> Get(string empType, string comID)
    {
        string query = "SELECT * FROM Company where CompanyID @ID and isActive = 1";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new List<Company>();
        if (empType == "3")
        {
            query = query.Replace("@ID", "IS NOT NULL");
        }
        else
        {
            query = query.Replace("@ID", "=" + comID);
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var data = new Company();
                    data.CompanyID = (string)reader["CompanyID"];
                    data.CompanyName = (string)reader["CompanyName"];
                    data.CompanyTypeID = (string)reader["CompanyTypeID"];
                    data.UpdatedBy = (string)reader["UpdatedBy"];
                    data.UpdatedDate = (DateTime)reader["UpdatedDate"];
                    res.Add(data);
                }
                reader.Close();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                throw;
            }
        }
    }
}
