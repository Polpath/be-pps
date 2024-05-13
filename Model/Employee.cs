namespace be.Model;

public class Employee
{
    public string? EmployeeID { get; set; }

    public string? EmployeeFirstName { get; set; }

    public string? EmployeeLastName { get; set; }

    public string? EmployeeCard { get; set; }

    public string? EmployeeTypeID { get; set; }

    public string? CompanyID { get; set; }

    public bool? isActive { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class EmployeeType
{
    public string? EmployeeTypeID { get; set; }

    public string? EmployeeTypeName { get; set; }

    public bool? isActive { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
