using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.CompanyController;

[ApiController]
[Route("[controller]")]
public class CompanyDetailController : ControllerBase
{
    public CompanyDetailController()
    {

    }

    [HttpGet(Name = "GetCompanyDetail/{companyID}")]
    public CompanyDetail Get(string companyID)
    {
        string query = "select c.CompanyName, ct.CompanyTypeID, ct.CompanyTypeName from Company c LEFT JOIN CompanyType ct on c.CompanyTypeID = ct.CompanyTypeID where CompanyID = '@ID' and c.isActive = 1";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";

        var res = new CompanyDetail();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            query = query.Replace("@ID", companyID);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res.CompanyName = (string)reader["CompanyName"];
                    res.CompanyTypeID = (string)reader["CompanyTypeID"];
                    res.CompanyTypeName = (string)reader["CompanyTypeName"];
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
