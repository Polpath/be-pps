namespace be.Model
{
    public class Login
    {
        public string Username {  get; set; }
        public string Password { get; set; }
    }

    public class Auth
    {
        public string? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCard { get; set; }
        public string? EmpTypeID { get; set; }
        public string? CompanyID { get; set; }

        public bool Status { get; set; } = false;
    }
}
