using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
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
