using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ATBasketRobotServer.Persistance.Configurations;
public sealed class BasketStatusConfiguration : IEntityTypeConfiguration<BasketStatus>
{
    public void Configure(EntityTypeBuilder<BasketStatus> builder)
    {
        //builder.HasOne<Customer>(sc => sc.Customer).WithMany<Customer>(s=>s.CustomerReferance).HasForeignKey(c => c.CustomerCode);
        builder.ToTable(TableNames.BasketStatuses);

        // builder.HasKey(p => p.Id);
    }
}