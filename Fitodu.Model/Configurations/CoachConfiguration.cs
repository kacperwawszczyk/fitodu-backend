using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Fitodu.Model.Configurations
{
    class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasKey(o => new
            {
                o.Id
            });

            builder.HasMany(x => x.PrivateNotes).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasMany(x => x.PublicNotes).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasMany(x => x.Trainings).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach);
            builder.HasMany(x => x.Exercises).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach);
            builder.HasMany(x => x.AwaitingTrainings).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasMany(x => x.WorkTimes).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasMany(x => x.CoachClients).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
            builder.HasMany(x => x.WeekPlans).WithOne(y => y.Coach).HasForeignKey(z => z.IdCoach).IsRequired();
        }
    }
}
