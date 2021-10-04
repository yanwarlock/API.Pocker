using API.Pocker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Pocker.Data.Configurations
{
    public class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("tb_userProfile");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name);

            builder.Property(t => t.IdUserIdentity);      

        }
    }
}
