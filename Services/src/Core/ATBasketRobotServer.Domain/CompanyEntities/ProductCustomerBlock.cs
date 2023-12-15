using System.ComponentModel.DataAnnotations.Schema;
namespace ATBasketRobotServer.Domain.CompanyEntities;
public class ProductCustomerBlock
{
    [ForeignKey(nameof(ProductId))]
    public string? ProductId { get; set; }
    public Product Product { get; set; }
    [ForeignKey(nameof(CustomerId))]
    public string? CustomerId { get; set; }
    public Customer Customer { get; set; }
}