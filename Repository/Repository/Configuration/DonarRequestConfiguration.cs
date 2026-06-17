using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository;

public class DonarRequestConfiguration : IEntityTypeConfiguration<DonarRequest>
{
    public void Configure(EntityTypeBuilder<DonarRequest> builder)
    {
        builder.ToTable("DonarRequests");
        builder.HasKey(x => x.DonarRequestId);
        builder.Property(x => x.DonarRequestId).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.RequesterId).IsRequired();
        builder.Property(x => x.DonarId).IsRequired();
        builder.Property(x => x.RequestDate).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.DonationTime).IsRequired();
        builder.Property(x => x.DonationDate).IsRequired();
        
        builder.HasOne(x => x.Requester)
            .WithMany(x => x.REQUEST)
            .HasForeignKey(x => x.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DonarUser)
            .WithMany(x => x.DONARREQUEST)
            .HasForeignKey(x => x.DonarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}