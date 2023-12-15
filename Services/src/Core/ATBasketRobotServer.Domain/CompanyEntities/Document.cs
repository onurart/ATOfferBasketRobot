using ATBasketRobotServer.Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATBasketRobotServer.Domain.CompanyEntities;
public sealed class Document : Entity
{
    public short? DocumetType { get; set; }
    public short? LineType { get; set; }
    public short? Billed { get; set; }
    public float Quantity { get; set; }
    public float? TlToltal { get; set; }
    [ForeignKey(nameof(ProductId))]
    public string? ProductId { get; set; }
    public long? ProductReferance { get; set; }
    public Product? Product { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public string? CustomerId { get; set; }
    public long? CustomerReferance { get; set; }
    public Customer? Customer { get; set; }
    public int? FicheReferance { get; set; }
    public DateTime DocumentDate { get; set; }
    public float? CurrencyRate { get; set; }
    public string? CurrencyType { get; set; }
    public float? CurrencyAmount { get; set; }
    public float? CurrencyAccordingToType { get; set; }
    public string? FICHENO { get; set; }
}