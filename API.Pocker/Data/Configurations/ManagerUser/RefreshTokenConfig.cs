using API.Pocker.Data.Entities.ManagerUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Data.Configurations.ManagerUser
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("tb_refreshToken");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Value);

            builder.Property(t => t.AccountId);

            builder.Property(t => t.Create);

            builder.Property(t => t.Expires);
        }
    }
}
