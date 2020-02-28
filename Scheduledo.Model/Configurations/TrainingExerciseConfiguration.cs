using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    class TrainingExerciseConfiguration : IEntityTypeConfiguration<TrainingExercise>
    {

        public void Configure(EntityTypeBuilder<TrainingExercise> builder)
        {
            builder.HasKey(o => new
            {
                o.IdTraining,
                o.IdExercise
            });
        }
    }
}
