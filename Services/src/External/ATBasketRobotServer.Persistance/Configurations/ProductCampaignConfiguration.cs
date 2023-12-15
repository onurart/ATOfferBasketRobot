using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ATBasketRobotServer.Persistance.Configurations;
public sealed class ProductCampaignConfiguration : IEntityTypeConfiguration<ProductCampaign>
{
    public void Configure(EntityTypeBuilder<ProductCampaign> builder)
    {
        builder.ToTable(TableNames.ProductCampaigns);
        builder.HasKey(p => p.Id);
    }
}