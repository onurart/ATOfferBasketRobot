using ATBasketRobotServer.Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATBasketRobotServer.Domain.CompanyEntities;
public class Offer : Entity
{
    [ForeignKey("ProductId")]
    public string? ProductId { get; set; }
    public Product? Product { get; set; }
    [ForeignKey("CustomerId")]
    public string? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public long? CustomerReferance { get; set; }
    public long? ProductReferance { get; set; }
    public int? Quantity { get; set; }
}