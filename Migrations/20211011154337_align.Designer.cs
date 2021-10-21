﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingOnBoard.MODELS;

namespace ParkingOnBoard.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211011154337_align")]
    partial class align
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ParkingOnBoard.MODELS.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StateID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("StateID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.ParkingSlot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int?>("StreetID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("StreetID");

                    b.ToTable("ParkingSlots");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.State", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.Street", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CityID")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SidesAvailable")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CarPlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.City", b =>
                {
                    b.HasOne("ParkingOnBoard.MODELS.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateID");

                    b.Navigation("State");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.ParkingSlot", b =>
                {
                    b.HasOne("ParkingOnBoard.MODELS.Street", "Street")
                        .WithMany("ParkingSlots")
                        .HasForeignKey("StreetID");

                    b.Navigation("Street");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.Street", b =>
                {
                    b.HasOne("ParkingOnBoard.MODELS.City", "City")
                        .WithMany("Streets")
                        .HasForeignKey("CityID");

                    b.Navigation("City");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.City", b =>
                {
                    b.Navigation("Streets");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.State", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("ParkingOnBoard.MODELS.Street", b =>
                {
                    b.Navigation("ParkingSlots");
                });
#pragma warning restore 612, 618
        }
    }
}
