using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Configurations
{
    class TrainingConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.TrainingExercises).WithOne(y => y.Training).HasForeignKey(z => z.IdTraining).IsRequired();
            //builder.HasMany(x => x.TrainingResults).WithOne(y => y.Training).HasForeignKey(z => z.IdTraining).IsRequired();
            builder.HasMany(x => x.Summaries).WithOne(y => y.Training).HasForeignKey(z => z.IdTraining);

            builder.HasOne(x => x.Coach).WithMany(y => y.Trainings).HasForeignKey(z => z.IdCoach);
            builder.HasOne(x => x.Client).WithMany(y => y.Trainings).HasForeignKey(z => z.IdClient);
        }
    }
}
