﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace csprj5._2.Migrations.Db
{
    [DbContext(typeof(DbContext))]
    [Migration("20211116074337_MonitorFileEnable")]
    partial class MonitorFileEnable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("csprjclib.Models.MonitorFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DelayInSecs")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MonitorFrequencies");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DelayId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DelayId");

                    b.ToTable("MonitoredFiles");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFileContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("ContentDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MonitoredFileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MonitoredFileId");

                    b.ToTable("MonitoredFileContents");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFileHash", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Hash")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("HashDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MonitoredFileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MonitoredFileId");

                    b.ToTable("MonitoredFileHashes");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFile", b =>
                {
                    b.HasOne("csprjclib.Models.MonitorFrequency", "Delay")
                        .WithMany()
                        .HasForeignKey("DelayId");

                    b.Navigation("Delay");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFileContent", b =>
                {
                    b.HasOne("csprjclib.Models.MonitoredFile", "MonitoredFile")
                        .WithMany()
                        .HasForeignKey("MonitoredFileId");

                    b.Navigation("MonitoredFile");
                });

            modelBuilder.Entity("csprjclib.Models.MonitoredFileHash", b =>
                {
                    b.HasOne("csprjclib.Models.MonitoredFile", "MonitoredFile")
                        .WithMany()
                        .HasForeignKey("MonitoredFileId");

                    b.Navigation("MonitoredFile");
                });
#pragma warning restore 612, 618
        }
    }
}
