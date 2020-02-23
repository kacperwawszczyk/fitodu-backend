using Scheduledo.Model.Entities;
using Scheduledo.Model.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Scheduledo.Model
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AwaitingTraining> AwaitingTrainings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachClient> CoachClients { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Maximum> Maximums { get; set; }
        public DbSet<PlannedTraining> PlannedTrainings { get; set; }
        public DbSet<PrivateNote> PrivateNotes { get; set; }
        public DbSet<PublicNote> PublicNotes { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingExercise> TrainingExercises { get; set; }
        public DbSet<TrainingResult> TrainingResults { get; set; }
        public DbSet<WorkTime> WorkTimes { get; set; }
        public DbSet<CreateClientToken> CreateClientTokens { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyAllConfigurations();
        }
    }
}