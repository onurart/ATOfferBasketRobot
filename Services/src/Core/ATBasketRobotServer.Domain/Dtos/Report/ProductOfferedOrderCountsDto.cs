namespace ATBasketRobotServer.Domain.Dtos.Report;
public class ProductOfferedOrderCountsDto
{
    public string Id { get; set; }
    public long? ProductReferance { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductGroup1 { get; set; }
    public string ProductGroup2 { get; set; }
    public string ProductGroup3 { get; set; }
    public string ProductGroup4 { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public double? MinOrder { get; set; }
    public decimal OfferedCount { get; set; }
    public decimal OrderedCount { get; set; }
}