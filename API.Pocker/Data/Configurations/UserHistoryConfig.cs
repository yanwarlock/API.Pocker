using API.Pocker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API.Pocker.Data.Configurations
{
    public class UserHistoryConfig : IEntityTypeConfiguration<UserProfileHistory>
    {
        public void Configure(EntityTypeBuilder<UserProfileHistory> builder)
        {
            builder.ToTable("tb_userHistory");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Date);

            builder.Property(t => t.Description);

            builder.HasOne( t => t.UserProfile)
           .WithMany(t => t.UserHistorys)
           .HasForeignKey(t => t.UserProfileId)
           .IsRequired(false);
        }
    }
}
