namespace ATBasketRobotServer.Domain.Dtos;

public class CustomerGroupDto
{
    public string Id { get; set; }
    public long? CustomerReferance { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductGroup1 { get; set; }
    public string? ProductGroup2 { get; set; }
    public string? ProductGroup3 { get; set; }
    public string? ProductGroup4 { get; set; }
    public bool? IsActive { get; set; }
}