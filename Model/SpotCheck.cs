namespace be.Model
{
    public class SpotCheck
    {
        public string? SpotCheckID { get; set; }

        public string? EmployeeID { get; set; }

        public string? CheckPointID { get; set; }

        public string? SpotCheckDate { get; set; }

        public string? SpotCheckImg { get; set; }

        public string? CompanyName { get; set; }

        public string? CheckPointName { get; set; }
    }
    
    public class SpotCheckExport
    {
        public List<string>? spotID { get; set; }
    }

    public class ExportSpot
    {
        public string? CheckPointName { get; set; }
        public string? CheckPointDesc { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
        public string? CompanyName { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
    }
}
