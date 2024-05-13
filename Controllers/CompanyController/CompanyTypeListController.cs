using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyTypeListController : ControllerBase
{
    public CompanyTypeListController()
    {

    }

    [HttpGet(Name = "GetCompanyTypeList/{empType}/{comType}")]
    public List<CompanyType> Get(string empType, string comType)
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string query = "SELECT * FROM CompanyType where CompanyTypeID = @ComType and isActive = 1";

        var res = new List<CompanyType>();

        if (empType == "3")
        {
            query = query.Replace("= @ComType", "IS NOT NULL");
        }
        else
        {
            query = query.Replace("@ComType", comType);
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
                    var data = new CompanyType();
                    data.CompanyTypeID = (string)reader["CompanyTypeID"];
                    data.CompanyTypeName = (string)reader["CompanyTypeName"];
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
