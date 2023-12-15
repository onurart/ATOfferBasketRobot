using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ATBasketRobotServer.Persistance.Configurations;
public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(TableNames.Documents);
        builder.HasKey(p => p.Id);
    }
}