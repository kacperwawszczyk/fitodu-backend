using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Configurations
{
    class SummaryConfiguration : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasOne(x => x.Client).WithMany(y => y.Summaries).HasForeignKey(z => z.IdClient);
        }
    }
}