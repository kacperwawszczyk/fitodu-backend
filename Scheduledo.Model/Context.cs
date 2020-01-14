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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyAllConfigurations();
        }
    }
}