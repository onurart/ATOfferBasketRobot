namespace ATBasketRobotServer.Domain.Dtos;
public sealed class DocumentDto
{
    public short? DocumetType { get; set; }
    public short? LineType { get; set; }
    public short? Billed { get; set; }
    public float Quantity { get; set; }
    public float? TlToltal { get; set; }
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
    public string? DocumentNo { get; set; }
    public DateTime? DocumentDate { get; set; }
    public int? FicheReferance { get; set; }
    public float? CurrencyRate { get; set; }
    public string? CurrencyType { get; set; }
    public float? CurrencyAmount { get; set; }
    public float? CurrencyAccordingToType { get; set; }
    public string? FICHENO { get; set; }
}