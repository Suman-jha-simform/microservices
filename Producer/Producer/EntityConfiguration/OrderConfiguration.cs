using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Producer.Entities;

namespace Producer.EntityConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Define table name
        builder.ToTable("ProducerOrders");

        // Define primary key
        builder.HasKey(o => o.Id);

        // Define properties
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd(); 

        builder.Property(o => o.ProductName)
            .IsRequired()
            .HasMaxLength(255); 

        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)") 
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()") 
            .IsRequired();
    }
}