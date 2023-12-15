using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ATBasketRobotServer.Persistance.Configurations;
public sealed class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable(TableNames.Logs);
        builder.HasKey(t => t.Id);
    }
}