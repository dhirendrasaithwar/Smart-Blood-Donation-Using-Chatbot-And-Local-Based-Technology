using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class BloodTypeConfig:IEntityTypeConfiguration<BloodType>
    {
        public void Configure(EntityTypeBuilder<BloodType> builder)
        {
            builder.ToTable("BloodType");
            builder.HasKey(x => x.BloodTypeId);
            builder.Property(x => x.BloodTypeId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.BloodTypes).IsRequired();

            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.UpdatedBy);
            builder.Property(r => r.UpdatedDate);
        }
    }
}
