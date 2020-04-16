﻿// <auto-generated />
using System;
using Fitodu.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fitodu.Model.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fitodu.Model.Entities.AwaitingTraining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("IdClient")
                        .IsRequired();

                    b.Property<string>("IdCoach")
                        .IsRequired();

                    b.Property<int>("Receiver");

                    b.Property<int>("Sender");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCoach");

                    b.ToTable("AwaitingTrainings");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Client", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressCountry");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("AddressPostalCode");

                    b.Property<string>("AddressState");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email");

                    b.Property<decimal?>("FatPercentage")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<decimal?>("Height")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<bool>("IsRegistered");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Coach", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressCountry");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("AddressPostalCode");

                    b.Property<string>("AddressState");

                    b.Property<long?>("CancelTimeHours");

                    b.Property<long?>("CancelTimeMinutes");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("EndOfLicenseDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Rules")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("VatIn");

                    b.HasKey("Id");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.CoachClient", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<int>("AvailableTrainings");

                    b.Property<string>("Place")
                        .HasColumnType("text");

                    b.Property<int>("PurchasedTrainings");

                    b.HasKey("IdCoach", "IdClient");

                    b.HasIndex("IdClient");

                    b.ToTable("CoachClients");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Company", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressCountry");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("AddressPostalCode");

                    b.Property<string>("AddressState");

                    b.Property<string>("BillingCustomerId");

                    b.Property<string>("BillingSubscriptionId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("EmailCounter");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Name");

                    b.Property<int>("Plan");

                    b.Property<DateTime?>("PlanExpiredOn");

                    b.Property<int>("SmsCounter");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("Url");

                    b.Property<string>("VatIn");

                    b.HasKey("Id");

                    b.HasIndex("Url")
                        .IsUnique()
                        .HasFilter("[Url] IS NOT NULL");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.CreateClientToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("ExpiresOn");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Token");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("CreateClientTokens");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.DayPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day");

                    b.Property<int>("WeekPlanId");

                    b.HasKey("Id");

                    b.HasIndex("WeekPlanId");

                    b.ToTable("DayPlans");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived");

                    b.Property<string>("Description");

                    b.Property<string>("IdCoach");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("IdCoach");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Maximum", b =>
                {
                    b.Property<int>("IdExercise");

                    b.Property<string>("IdClient");

                    b.Property<string>("Max");

                    b.HasKey("IdExercise", "IdClient");

                    b.HasIndex("IdClient");

                    b.ToTable("Maximums");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.PlannedTraining", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<int>("IdTraining");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime");

                    b.HasKey("IdCoach", "IdTraining");

                    b.ToTable("PlannedTrainings");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.PrivateNote", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("IdCoach", "IdClient");

                    b.HasIndex("IdClient");

                    b.ToTable("PrivateNotes");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.PublicNote", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("IdCoach", "IdClient");

                    b.HasIndex("IdClient");

                    b.ToTable("PublicNotes");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("ExpiresOn");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Token");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Summary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("IdTraining");

                    b.Property<decimal?>("WeightChange")
                        .HasColumnType("decimal(3, 2)");

                    b.HasKey("Id");

                    b.HasIndex("IdTraining");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("IdClient");

                    b.Property<string>("IdCoach");

                    b.Property<string>("Name");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCoach");

                    b.ToTable("Trainings");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.TrainingExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdExercise");

                    b.Property<int>("IdTraining");

                    b.Property<int>("Repetitions");

                    b.Property<int>("RepetitionsResult");

                    b.Property<TimeSpan?>("Time");

                    b.Property<TimeSpan?>("TimeResult");

                    b.HasKey("Id");

                    b.HasIndex("IdExercise");

                    b.HasIndex("IdTraining");

                    b.ToTable("TrainingExercises");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.TrainingResult", b =>
                {
                    b.Property<int>("IdExercise");

                    b.Property<int>("IdTraining");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Id");

                    b.Property<int>("IdTrainingExercise");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int>("Repetitions");

                    b.Property<TimeSpan?>("Time");

                    b.HasKey("IdExercise", "IdTraining");

                    b.HasAlternateKey("Id");

                    b.HasIndex("IdTrainingExercise")
                        .IsUnique();

                    b.ToTable("TrainingResults");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<long>("CompanyId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ResetToken");

                    b.Property<DateTime?>("ResetTokenExpiresOn");

                    b.Property<int>("Role");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("SubscriberId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WeekPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdCoach")
                        .IsRequired();

                    b.Property<bool>("IsDefault");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("IdCoach");

                    b.ToTable("WeekPlans");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WorkTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Day")
                        .HasColumnType("date");

                    b.Property<TimeSpan?>("EndHour");

                    b.Property<string>("IdCoach")
                        .IsRequired();

                    b.Property<TimeSpan?>("StartHour");

                    b.HasKey("Id");

                    b.HasIndex("IdCoach");

                    b.ToTable("WorkTimes");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WorkoutTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DayPlanId");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("DayPlanId");

                    b.ToTable("WorkoutTimes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.AwaitingTraining", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("AwaitingTrainings")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("AwaitingTrainings")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.CoachClient", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("ClientCoaches")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("CoachClients")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.CreateClientToken", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.DayPlan", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.WeekPlan", "WeekPlan")
                        .WithMany("DayPlans")
                        .HasForeignKey("WeekPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Exercise", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("Exercises")
                        .HasForeignKey("IdCoach");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Maximum", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("Maximums")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Exercise", "Exercise")
                        .WithMany("Maximums")
                        .HasForeignKey("IdExercise")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.PrivateNote", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("PrivateNotes")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("PrivateNotes")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.PublicNote", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("PublicNotes")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("PublicNotes")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.RefreshToken", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Summary", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Training", "Training")
                        .WithMany("Summaries")
                        .HasForeignKey("IdTraining")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.Training", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Client", "Client")
                        .WithMany("Trainings")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("Trainings")
                        .HasForeignKey("IdCoach");
                });

            modelBuilder.Entity("Fitodu.Model.Entities.TrainingExercise", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Exercise", "Exercise")
                        .WithMany("TrainingExercises")
                        .HasForeignKey("IdExercise")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.Training", "Training")
                        .WithMany("TrainingExercises")
                        .HasForeignKey("IdTraining")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.User", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WeekPlan", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("WeekPlans")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WorkTime", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.Coach", "Coach")
                        .WithMany("WorkTimes")
                        .HasForeignKey("IdCoach")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fitodu.Model.Entities.WorkoutTime", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.DayPlan", "DayPlan")
                        .WithMany("WorkoutTimes")
                        .HasForeignKey("DayPlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fitodu.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Fitodu.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
