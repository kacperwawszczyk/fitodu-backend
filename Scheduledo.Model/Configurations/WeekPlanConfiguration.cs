using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    class WeekPlanConfiguration : IEntityTypeConfiguration<WeekPlan>
    {
        public void Configure(EntityTypeBuilder<WeekPlan> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });
        }
    }
}
