using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Fitodu.Model.Configurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.Maximums).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PrivateNote).WithOne(y => y.Client).HasForeignKey<PrivateNote>(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PublicNote).WithOne(y => y.Client).HasForeignKey<PublicNote>(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Trainings).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.AwaitingTrainings).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.ClientCoaches).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Summaries).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
