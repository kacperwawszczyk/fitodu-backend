using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Scheduledo.Model.Configurations
{
    class MaximumConfiguration : IEntityTypeConfiguration<Maximum>
    {
        public void Configure(EntityTypeBuilder<Maximum> builder)
        {
            builder.HasKey(o => new
            {
                o.IdExercise,
                o.IdClient
            });
        }
    }
}
