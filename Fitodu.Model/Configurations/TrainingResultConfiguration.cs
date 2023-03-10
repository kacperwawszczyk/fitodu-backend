using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class TrainingResultConfiguration : IEntityTypeConfiguration<TrainingResult>
    {
        public void Configure(EntityTypeBuilder<TrainingResult> builder)
        {
            builder.HasKey(o => new
            {
                o.IdExercise,
                o.IdTraining
            });

            builder.HasIndex(o => new
            {
                o.IdTrainingExercise
            }).IsUnique();

            //builder.HasOne(x => x.Training).WithMany(y => y.TrainingResults).HasForeignKey(z => z.IdTraining).IsRequired();

            //builder.HasOne(x => x.Exercise).WithOne(y => y.TrainingResult).HasForeignKey<TrainingResult>(z => z.IdExercise).IsRequired();
            //builder.HasOne(x => x.TrainingExercise).WithOne(y => y.TrainingResult).HasForeignKey<TrainingResult>(z => z.IdTrainingExercise).IsRequired();
        }
    }
}
