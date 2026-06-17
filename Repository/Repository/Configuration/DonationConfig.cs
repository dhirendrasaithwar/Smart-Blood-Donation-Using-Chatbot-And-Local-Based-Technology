using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class DonationConfig:IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donation");
            builder.HasKey(d => d.DonationID);
            builder.Property(x=>x.DonationID).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x=>x.DonationDate).IsRequired();
            builder.Property(x=>x.Location).IsRequired();

            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.UpdatedBy);
            builder.Property(r => r.UpdatedDate);

            builder.HasOne(x=>x.USER)
                .WithMany(x=>x.DONATION).HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x => x.BloodType)
                .WithMany(x => x.Donation)
                .HasForeignKey(x => x.BloodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
