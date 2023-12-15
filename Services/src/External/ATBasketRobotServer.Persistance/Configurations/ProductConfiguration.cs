using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ATBasketRobotServer.Persistance.Configurations;
public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);
        builder.HasKey(p => p.Id);
    }
}