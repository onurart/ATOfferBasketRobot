namespace ATBasketRobotServer.Domain.Dtos.Report;
public class DocumentDetailDto
{
    public int ProductReferance { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductGroup1 { get; set; }
    public string ProductGroup2 { get; set; }
    public string ProductGroup3 { get; set; }
    public string ProductGroup4 { get; set; }
    public int CustomerReferance { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public DateTime DocumentDate { get; set; }
    public string CurrencyType { get; set; }
    public decimal CurrencyAmount { get; set; }
    public decimal TotalTlTotal { get; set; }
    public decimal TotalQuantity { get; set; }
}
