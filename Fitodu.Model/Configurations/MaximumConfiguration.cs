using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Configurations
{
    class MaximumConfiguration : IEntityTypeConfiguration<Maximum>
    {
        public void Configure(EntityTypeBuilder<Maximum> builder)
        {
            builder.HasKey(o => new
            {
                o.IdExercise,
                o.IdClient
            });

            builder.HasOne(x => x.Client).WithMany(y => y.Maximums).HasForeignKey(z => z.IdClient);
            builder.HasOne(x => x.Exercise).WithMany(y => y.Maximums).HasForeignKey(z => z.IdExercise);
        }
    }
}
