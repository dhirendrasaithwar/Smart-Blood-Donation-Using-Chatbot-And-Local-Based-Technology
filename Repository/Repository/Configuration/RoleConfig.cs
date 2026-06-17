using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class RoleConfig:IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.RoleId);
            builder.Property(x=>x.RoleId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.RoleType).IsRequired();
            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.UpdatedBy);
            builder.Property(r => r.UpdatedDate);

        }
    }
}
