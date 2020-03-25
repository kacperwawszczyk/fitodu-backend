using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Configurations
{
    public class CoachClientConfiguration : IEntityTypeConfiguration<CoachClient>
    {
        public void Configure(EntityTypeBuilder<CoachClient> builder)
        {
            builder.HasKey(o => new
            {
                o.IdCoach,
                o.IdClient
            });

            builder.HasOne(x => x.Coach).WithMany(y => y.CoachClients).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasOne(x => x.Client).WithMany(y => y.ClientCoaches).HasForeignKey(z => z.IdClient).IsRequired();
        }
    }
}