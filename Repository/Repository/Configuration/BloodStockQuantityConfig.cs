using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository;

public class BloodStockQuantityConfig : IEntityTypeConfiguration<BloodStock>
{
    public void Configure(EntityTypeBuilder<BloodStock> builder)
    {
        builder.ToTable("BloodStock");
        builder.HasKey(x =>x.BloodStockId);
        builder.Property(x => x.BloodStockId).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.ReserveQuantity).HasMaxLength(100).IsRequired();
        builder.Property(x => x.BloodTypeId).IsRequired();
        builder.Property(x => x.LastUpdated).HasColumnType("datetime").IsRequired();
        
        builder.HasOne(x => x.BLOODTYPES)
            .WithMany(x => x.BLOODSTOCK)
            .HasForeignKey(x => x.BloodTypeId)
            .OnDelete(DeleteBehavior.Restrict);;
        
        builder.HasIndex(x => x.BloodTypeId).IsUnique();
        
    }
}