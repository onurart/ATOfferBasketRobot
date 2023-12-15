using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Persistance.Constans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATBasketRobotServer.Persistance.Configurations
{
    public class ProductCustomerBlockConfiguration : IEntityTypeConfiguration<ProductCustomerBlock>
    {
        public void Configure(EntityTypeBuilder<ProductCustomerBlock> builder)
        {
            builder.ToTable(TableNames.ProductCustomerBlocks);

            // Birleşik anahtar olarak ProductId ve CustomerId'yi tanımlayalım.
            builder.HasKey(pcb => new { pcb.ProductId, pcb.CustomerId });

            // ProductId'nin Product tablosuyla ilişkilendirilmesi
            builder.HasOne(pcb => pcb.Product)
                   .WithMany()
                   .HasForeignKey(pcb => pcb.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);  // Engelleme silindiğinde ürünün silinmemesi için

            // CustomerId'nin Customer tablosuyla ilişkilendirilmesi
            builder.HasOne(pcb => pcb.Customer)
                   .WithMany()
                   .HasForeignKey(pcb => pcb.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
