using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Scheduledo.Model.Configurations
{
    public class CoachClientConfiguration : IEntityTypeConfiguration<CoachClient>
    {
        public void Configure(EntityTypeBuilder<CoachClient> builder)
        {
            builder.HasKey(o => new
            {
                o.IdCoach,
                o.IdClient
            });
        }
    }
}