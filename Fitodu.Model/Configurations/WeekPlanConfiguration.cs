using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class WeekPlanConfiguration : IEntityTypeConfiguration<WeekPlan>
    {
        public void Configure(EntityTypeBuilder<WeekPlan> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.DayPlans).WithOne(y => y.WeekPlan);

            builder.HasOne(x => x.Coach).WithMany(y => y.WeekPlans).HasForeignKey(z => z.IdCoach).IsRequired();
        }
    }
}
