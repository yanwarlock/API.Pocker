using API.Pocker.Data.Entities;
using API.Pocker.Data.Entities.ManagerUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserProfileHistory> UserProfileHistorys { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Votes> Votes { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
