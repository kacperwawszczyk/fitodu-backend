using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    public class PrivateNoteConfiguration : IEntityTypeConfiguration<PrivateNote>
    {
        public void Configure(EntityTypeBuilder<PrivateNote> builder)
        {
            builder.HasKey(o => new
            {
                o.IdCoach,
                o.IdClient
            });
        }
    }
}
