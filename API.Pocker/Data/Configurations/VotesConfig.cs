using API.Pocker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Pocker.Data.Configurations
{
    public class VotesConfig : IEntityTypeConfiguration<Votes>
    {
        public void Configure(EntityTypeBuilder<Votes> builder)
        {
            builder.ToTable("tb_votes");

            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.UserProfile);

            builder.HasOne(t => t.Cards);
              
        }
    }
}
