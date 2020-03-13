using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    class WorkoutTimeConfiguration : IEntityTypeConfiguration<WorkoutTime>
    {
        public void Configure(EntityTypeBuilder<WorkoutTime> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });
        }
    }
}
