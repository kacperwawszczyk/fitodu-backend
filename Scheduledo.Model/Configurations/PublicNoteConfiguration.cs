using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Model.Configurations
{
    public class PublicNoteConfiguration : IEntityTypeConfiguration<PublicNote>
    {
        public void Configure(EntityTypeBuilder<PublicNote> builder)
        {
            builder.HasKey(o => new
            {
                o.IdCoach,
                o.IdClient
            });
        }
    }

}
