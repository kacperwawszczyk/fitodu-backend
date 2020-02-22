using Scheduledo.Model.Entities;
using Scheduledo.Model.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Scheduledo.Model
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<PrivateNote> PrivateNotes { get; set; }

        public DbSet<Coach> Coach { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PrivateNote>()
            .HasKey(x => new { x.IdCoach, x.IdClient });

            builder.ApplyAllConfigurations();
        }
    }
}