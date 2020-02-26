﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Scheduledo.Model;

namespace Scheduledo.Model.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200226153951_SmallClientFixForDecimalValues")]
    partial class SmallClientFixForDecimalValues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("Scheduledo.Model.Entities.AwaitingTraining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("IdClient")
                        .IsRequired();

                    b.Property<string>("IdCoach")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AwaitingTrainings");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.Client", b =>
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

                    b.Property<decimal?>("FatPercentage")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<decimal?>("Height")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<bool>("IsRegistered");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.Coach", b =>
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

                    b.Property<string>("TimeToResign");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("VatIn");

                    b.HasKey("Id");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.CoachClient", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<int>("AvailableTrainings");

                    b.Property<string>("Place")
                        .HasColumnType("text");

                    b.Property<int>("PurchasedTrainings");

                    b.Property<string>("TimeToResign")
                        .HasMaxLength(30);

                    b.HasKey("IdCoach", "IdClient");

                    b.ToTable("CoachClients");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.Company", b =>
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

            modelBuilder.Entity("Scheduledo.Model.Entities.CreateClientToken", b =>
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

            modelBuilder.Entity("Scheduledo.Model.Entities.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("IdCoach");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.Maximum", b =>
                {
                    b.Property<int>("IdExercise");

                    b.Property<string>("IdClient");

                    b.Property<string>("Max");

                    b.HasKey("IdExercise", "IdClient");

                    b.ToTable("Maximums");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.PlannedTraining", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<int>("IdTraining");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime");

                    b.HasKey("IdCoach", "IdTraining");

                    b.ToTable("PlannedTrainings");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.PrivateNote", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("IdCoach", "IdClient");

                    b.ToTable("PrivateNotes");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.PublicNote", b =>
                {
                    b.Property<string>("IdCoach");

                    b.Property<string>("IdClient");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("IdCoach", "IdClient");

                    b.ToTable("PublicNotes");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.RefreshToken", b =>
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

            modelBuilder.Entity("Scheduledo.Model.Entities.Summary", b =>
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

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("IdClient");

                    b.Property<string>("IdCoach");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Trainings");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.TrainingExercise", b =>
                {
                    b.Property<int>("IdTraining");

                    b.Property<int>("IdExercise");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Repetitions");

                    b.Property<TimeSpan?>("Time");

                    b.Property<string>("TrainerNote")
                        .HasColumnType("text");

                    b.HasKey("IdTraining", "IdExercise");

                    b.ToTable("TrainingExercises");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.TrainingResult", b =>
                {
                    b.Property<int>("IdExercise");

                    b.Property<int>("IdTraining");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int>("Repetitions");

                    b.Property<TimeSpan?>("Time");

                    b.HasKey("IdExercise", "IdTraining");

                    b.ToTable("TrainingResults");
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.User", b =>
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

            modelBuilder.Entity("Scheduledo.Model.Entities.WorkTime", b =>
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

                    b.ToTable("WorkTimes");
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
                    b.HasOne("Scheduledo.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Scheduledo.Model.Entities.User")
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

                    b.HasOne("Scheduledo.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Scheduledo.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.CreateClientToken", b =>
                {
                    b.HasOne("Scheduledo.Model.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.RefreshToken", b =>
                {
                    b.HasOne("Scheduledo.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheduledo.Model.Entities.User", b =>
                {
                    b.HasOne("Scheduledo.Model.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
