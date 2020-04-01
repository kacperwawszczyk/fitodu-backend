using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Fitodu.Model.Configurations
{
    class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.Maximums).WithOne(y => y.Exercise).HasForeignKey(z => z.IdExercise);
            builder.HasMany(x => x.TrainingExercises).WithOne(y => y.Exercise).HasForeignKey(z => z.IdExercise).IsRequired();

            builder.HasOne(x => x.Coach).WithMany(y => y.Exercises).HasForeignKey(z => z.IdCoach);

            //builder.HasOne(x => x.TrainingResult).WithOne(y => y.Exercise).HasForeignKey<TrainingResult>(z => z.IdExercise).IsRequired();
        }
    }
}
