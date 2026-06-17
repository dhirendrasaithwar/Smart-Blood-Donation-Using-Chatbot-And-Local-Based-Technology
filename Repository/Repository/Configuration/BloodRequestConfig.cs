using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class BloodRequestConfig:IEntityTypeConfiguration<BloodRequest>
    {
        public void Configure(EntityTypeBuilder<BloodRequest> builder)
        {
            builder.ToTable("BloodRequest");
            builder.HasKey(x => x.BloodRequestId);
            builder.Property(x => x.BloodRequestId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BloodTypeId).IsRequired();
            builder.Property(x => x.BloodType).IsRequired();
            builder.Property(x=>x.Quantity).IsRequired();
            builder.Property(x=>x.RequiredDate).IsRequired();
            builder.Property(x=>x.UrgencyType).IsRequired();
            builder.Property(x=>x.Location).IsRequired();
            builder.Property(x=>x.Status).IsRequired();

            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.UpdatedBy);
            builder.Property(r => r.UpdatedDate);

            builder.HasOne(x=>x.BLOODTYPE).WithMany(x=>x.BLOODREQUEST)
                .HasForeignKey(x=>x.BloodTypeId).OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x => x.Users).WithMany(x => x.BLOODREQUEST)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
