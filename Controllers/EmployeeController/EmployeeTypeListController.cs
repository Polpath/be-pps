using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace be.Controllers.EmployeeController;

[ApiController]
[Route("[controller]")]
public class EmployeeTypeListController : ControllerBase
{
    public EmployeeTypeListController()
    {

    }

    [HttpGet(Name = "GetEmployeeTypeList/{empType}")]
    public List<EmployeeType> Get(string empType)
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string query = "SELECT * FROM EmployeeType where EmpTypeID = @empType and isActive = 1";

        var res = new List<EmployeeType>();

        if (empType == "3")
        {
            query = query.Replace("= @empType", "IS NOT NULL");
        }
        else
        {
            query = query.Replace("@empType", empType);
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
                    var data = new EmployeeType();
                    data.EmployeeTypeID = (string)reader["EmpTypeID"];
                    data.EmployeeTypeName = (string)reader["EmpTypeName"];
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
