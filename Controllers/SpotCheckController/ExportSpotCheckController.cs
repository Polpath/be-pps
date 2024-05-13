using be.Controllers.CompanyController;
using be.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace be.Controllers.SpotCheckController;

[ApiController]
[Route("[controller]")]
public class ExportSpotCheckController : ControllerBase
{
    public ExportSpotCheckController()
    {

    }

    [HttpGet(Name = "ExportSpotCheck/{spotID}")]
    public IActionResult ExportToExcel(string spotID)
    {
        string query = @"select * from SpotCheck s left join Employee e on s.EmployeeID = e.EmployeeID left join [CheckPoint] c on s.CheckPointID = c.CheckPointID left join Company com on e.CompanyID = com.CompanyID where SpotID in (@ID)";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PPSGUARD;Integrated Security=True;";
        string id = "";

        var result = new List<ExportSpot>();

        query = query.Replace("@ID", spotID);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var data = new ExportSpot();
                    data.CheckPointName = (string)reader["CheckPointName"];
                    data.CheckPointDesc = (string)reader["CheckPointDesc"];
                    data.EmployeeFirstName = (string)reader["EmployeeFirstname"];
                    data.EmployeeLastName = (string)reader["EmployeeLastname"];
                    data.CompanyName = (string)reader["CompanyName"];

                    string dateData = ((DateTime)reader["SpotCheckDate"]).ToString();
                    DateTime datePlace = DateTime.Parse(dateData);
                    data.Date = datePlace.ToString("dd-MM-yyyy ");

                    string timeData = ((DateTime)reader["SpotCheckDate"]).ToString();
                    DateTime timePlace = DateTime.Parse(timeData);
                    data.Time = timePlace.ToString("HH:mm:ss");
                    result.Add(data);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to LocalDB (LocalDB)\\MSSQLLocalDB: " + ex.Message);
                throw;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");
                worksheet.Cells[1, 1].Value = "ชื่อ";
                worksheet.Cells[1, 2].Value = "นามสกุล";
                worksheet.Cells[1, 3].Value = "หน่วยงาน";
                worksheet.Cells[1, 4].Value = "ชื่อจุตรวจ";
                worksheet.Cells[1, 5].Value = "รายละเอียดจุดตรวจ";
                worksheet.Cells[1, 6].Value = "วันที่ตรวจ";
                worksheet.Cells[1, 7].Value = "เวลาที่ตรวจ";
                var row = 2;
                foreach(var i in result)
                {
                    worksheet.Cells[row, 1].Value = i.EmployeeFirstName;
                    worksheet.Cells[row, 2].Value = i.EmployeeLastName;
                    worksheet.Cells[row, 3].Value = i.CompanyName;
                    worksheet.Cells[row, 4].Value = i.CheckPointName;
                    worksheet.Cells[row, 5].Value = i.CheckPointDesc;
                    worksheet.Cells[row, 6].Value = i.Date;
                    worksheet.Cells[row, 7].Value = i.Time;
                    row++;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SpotCheckReport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            }
        }
    }
}
