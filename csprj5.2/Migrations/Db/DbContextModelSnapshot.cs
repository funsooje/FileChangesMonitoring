﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace csprj5._2.Migrations.Db
{
    [DbContext(typeof(DbContext))]
    partial class DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("csprj5._2.Models.MonitorFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DelayInSecs")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MonitorFrequencies");
                });

            modelBuilder.Entity("csprj5._2.Models.MonitoredFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DelayId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DelayId");

                    b.ToTable("MonitoredFiles");
                });

            modelBuilder.Entity("csprj5._2.Models.MonitoredFile", b =>
                {
                    b.HasOne("csprj5._2.Models.MonitorFrequency", "Delay")
                        .WithMany()
                        .HasForeignKey("DelayId");

                    b.Navigation("Delay");
                });
#pragma warning restore 612, 618
        }
    }
}