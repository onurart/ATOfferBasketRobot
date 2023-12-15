using ATBasketRobotServer.Domain.Abstractions;
namespace ATBasketRobotServer.Domain.CompanyEntities;
public sealed class Customer : Entity
{
    public long? CustomerReferance { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public bool? IsActive { get; set; } = true;
}