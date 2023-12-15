using ATBasketRobotServer.Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATBasketRobotServer.Domain.CompanyEntities;
public sealed class BasketStatus : Entity
{
    public int BasketStatusReferance { get; set; }
    [ForeignKey(nameof(ProductId))]
    public string? ProductId { get; set; }
    public int? ProductReferance { get; set; }
    public string? ProductCode { get; set; }
    public Product? Product { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public string? CustomerId { get; set; }
    public int? CustomerReferance { get; set; }
    public string? CustomerCode { get; set; }
    public Customer? Customer { get; set; }
}