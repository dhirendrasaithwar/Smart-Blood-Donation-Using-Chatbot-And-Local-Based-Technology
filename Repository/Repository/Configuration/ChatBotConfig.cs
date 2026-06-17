using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository;

public class ChatBotConfig : IEntityTypeConfiguration<ChatBot>
{
    public void Configure(EntityTypeBuilder<ChatBot> builder)
    {
        builder.ToTable("ChatBots");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Answer).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(200).IsRequired();
        
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
    }
}