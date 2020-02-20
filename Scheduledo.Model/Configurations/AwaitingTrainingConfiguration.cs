using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Scheduledo.Model.Configurations
{
    class AwaitingTrainingConfiguration : IEntityTypeConfiguration<AwaitingTraining>
    {
        public void Configure(EntityTypeBuilder<AwaitingTraining> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });
        }
    }
}
