using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Configurations
{
    class AwaitingTrainingConfiguration : IEntityTypeConfiguration<AwaitingTraining>
    {
        public void Configure(EntityTypeBuilder<AwaitingTraining> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasOne(x => x.Coach).WithMany(y => y.AwaitingTrainings).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasOne(x => x.Client).WithMany(y => y.AwaitingTrainings).HasForeignKey(z => z.IdClient).IsRequired();
        }
    }
}
