using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
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

            builder.HasOne(x => x.Coach).WithMany(y => y.PrivateNotes).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasOne(x => x.Client).WithOne(y => y.PrivateNote).HasForeignKey<PrivateNote>(z => z.IdClient).IsRequired();
        }
    }
}
