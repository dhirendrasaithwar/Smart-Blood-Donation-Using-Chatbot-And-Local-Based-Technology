using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class UserRoleConfig:IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(x => x.UserRoleId);
            builder.Property(x=>x.UserRoleId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();

            builder.HasOne(x=>x.USER).WithMany(x=>x.USERROLE)
                .HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ROLE).WithMany(x => x.USERROLE)
                .HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
