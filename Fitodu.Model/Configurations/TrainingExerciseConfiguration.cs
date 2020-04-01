using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class TrainingExerciseConfiguration : IEntityTypeConfiguration<TrainingExercise>
    {

        public void Configure(EntityTypeBuilder<TrainingExercise> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasOne(x => x.Training).WithMany(y => y.TrainingExercises).HasForeignKey(z => z.IdTraining).IsRequired();

            builder.HasOne(x => x.Exercise).WithOne(y => y.TrainingExercise).HasForeignKey<TrainingExercise>(z => z.IdExercise).IsRequired();
            builder.HasOne(x => x.TrainingResult).WithOne(y => y.TrainingExercise).HasForeignKey<TrainingResult>(z => z.IdTrainingExercise).IsRequired();
        }
    }
}
