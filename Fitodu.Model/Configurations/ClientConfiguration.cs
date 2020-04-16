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
            builder.HasMany(x => x.PrivateNotes).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.PublicNotes).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Trainings).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.AwaitingTrainings).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.ClientCoaches).WithOne(y => y.Client).HasForeignKey(z => z.IdClient).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
