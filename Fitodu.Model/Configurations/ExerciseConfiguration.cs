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
        }
    }
}
