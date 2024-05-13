namespace be.Model
{
    public class CheckPoint
    {
        public string? CheckPointID { get; set; }

        public string? CheckPointName { get; set; }

        public string? CheckPointDesc { get; set; }

        public string? CompanyID { get; set; }

        public bool? isActive { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
