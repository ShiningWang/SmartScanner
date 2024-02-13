﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartScannerBackend.DataAccess;

#nullable disable

namespace SmartScannerBackend.Migrations
{
    [DbContext(typeof(SmartScannerDbContext))]
    [Migration("20240208152347_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("SmartScannerBackend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Disqualified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Relational:JsonPropertyName", "Role");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "d0545a81-b429-4690-9c84-003bb4226439",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a0545fad-ae93-4a6d-b43d-6794ea7abc72",
                            Disqualified = false,
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            Password = "lZy/kPV0YR/B4vTeILI2nTKQJZgMnZfJni77KrZgo14=",
                            PhoneNumberConfirmed = false,
                            Role = 1,
                            Salt = "IlinI6apeJmg1WSJOVPbSA==",
                            SecurityStamp = "cc1b0283-d25a-445b-95ca-83398f4998b6",
                            TwoFactorEnabled = false,
                            UserName = "shiningdevusername"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
