namespace be.Model;

public class Company
{
    public string? CompanyID { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyTypeID { get; set; }

    public bool? isActive { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CompanyType
{
    public string? CompanyTypeID { get; set; }

    public string? CompanyTypeName { get; set; }

    public bool? isActive { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CompanyDetail
{
    public string? CompanyName { get; set; }

    public string? CompanyTypeID { get; set; }

    public string? CompanyTypeName { get; set; }
}
