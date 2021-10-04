using API.Pocker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API.Pocker.Data.Configurations
{
    public class CardsConfig : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("tb_cards");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Value);
        }
    }
}
