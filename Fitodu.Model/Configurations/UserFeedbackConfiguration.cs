using Fitodu.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Configurations
{
    class UserFeedbackConfiguration : IEntityTypeConfiguration<UserFeedback>
    {
        public void Configure(EntityTypeBuilder<UserFeedback> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });
            
        }
    }
}
