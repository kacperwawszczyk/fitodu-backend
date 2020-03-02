using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
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
        }
    }
}
