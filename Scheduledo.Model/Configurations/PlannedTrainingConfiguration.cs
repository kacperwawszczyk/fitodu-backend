using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    class PlannedTrainingConfiguration : IEntityTypeConfiguration<PlannedTraining>
    {
        public void Configure(EntityTypeBuilder<PlannedTraining> builder)
        {
            builder.HasKey(o => new
            {
                o.IdCoach,
                o.IdTraining
            });
        }
    }
}
