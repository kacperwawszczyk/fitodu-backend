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
        }
    }
}
