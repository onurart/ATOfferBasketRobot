namespace ATBasketRobotServer.Domain.Dtos;
public sealed class OfferDto
{
    public string? ProductId { get; set; }
    public long? ProductReferance { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? ProductGroup1 { get; set; }
    public string? ProductGroup2 { get; set; }
    public string? ProductGroup3 { get; set; }
    public string? ProductGroup4 { get; set; }
    public string? CustomerId { get; set; }
    public long? CustomerReferance { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public int? Quantity { get; set; }
}