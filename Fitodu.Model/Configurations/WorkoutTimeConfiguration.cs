using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class WorkoutTimeConfiguration : IEntityTypeConfiguration<WorkoutTime>
    {
        public void Configure(EntityTypeBuilder<WorkoutTime> builder)
        {
            //builder.HasKey(o => new
            //{
            //    o.Id
            //});

            builder.HasOne(x => x.DayPlan).WithMany(y => y.WorkoutTimes).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
