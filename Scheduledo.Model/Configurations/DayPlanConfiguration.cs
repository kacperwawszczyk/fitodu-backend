using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    class DayPlanConfiguration : IEntityTypeConfiguration<DayPlan>
    {
        public void Configure(EntityTypeBuilder<DayPlan> builder)
        {
            //builder.HasKey(o => new
            //{
            //    o.Id
            //});

            builder.HasOne(x => x.WeekPlan).WithMany(y => y.DayPlans).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
