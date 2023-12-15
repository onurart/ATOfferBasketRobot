namespace ATBasketRobotServer.Domain.Dtos.Report;
public class ProductDto
{
    public string Id { get; set; }
    public long?   ProductReferance { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductGroup1 { get; set; }
    public string? ProductGroup2 { get; set; }
    public string? ProductGroup3 { get; set; }
    public string? ProductGroup4 { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? IsDelete { get; set; } = false;
    public double MinOrder { get; set; }
}