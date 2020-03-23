﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
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