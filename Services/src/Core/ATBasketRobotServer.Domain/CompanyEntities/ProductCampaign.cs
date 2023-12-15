using ATBasketRobotServer.Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATBasketRobotServer.Domain.CompanyEntities;
public sealed class ProductCampaign : Entity
{
    [ForeignKey(nameof(ProductId))]
    public string? ProductId { get; set; }
    public int? ProductReferance { get; set; }
    public string? ProductCode { get; set; }
    public Product? Product { get; set; }
}