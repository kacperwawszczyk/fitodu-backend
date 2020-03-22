using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Fitodu.Model.Configurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });
        }
    }
}
