using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Repository
{
    public class UserConfig:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.UserId);
            builder.Property(x=>x.UserId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.FullName).IsRequired();
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x=>x.Email).IsRequired();
            builder.Property(x=>x.Password).IsRequired();
            builder.Property(x => x.IsUserVerify).IsRequired();
            builder.Property(x => x.BloodTypeId).IsRequired();
            builder.Property(x=>x.BloodType).IsRequired();
            builder.Property(x=>x.DateOfBirth).IsRequired();
            builder.Property(x=>x.IsPreviousDonate).IsRequired();
            builder.Property(x=>x.LastDonationDate);

            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.UpdatedBy);
            builder.Property(r => r.UpdatedDate);

            builder.HasOne(x=>x.BLOODTYPE)
                .WithMany(x=>x.USER).HasForeignKey(x=>x.BloodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
