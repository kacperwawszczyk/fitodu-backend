using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    public class CreateClientTokenConfiguration : IEntityTypeConfiguration<CreateClientToken>
    {
        public void Configure(EntityTypeBuilder<CreateClientToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Client).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
