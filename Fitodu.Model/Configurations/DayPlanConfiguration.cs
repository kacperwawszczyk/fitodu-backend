using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class DayPlanConfiguration : IEntityTypeConfiguration<DayPlan>
    {
        public void Configure(EntityTypeBuilder<DayPlan> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.WorkoutTimes).WithOne(y => y.DayPlan);

            builder.HasOne(x => x.WeekPlan).WithMany(y => y.DayPlans).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
