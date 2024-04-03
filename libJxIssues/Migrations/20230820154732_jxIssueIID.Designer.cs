﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using libJxIssues.Models;

#nullable disable

namespace libJxIssues.Migrations
{
    [DbContext(typeof(jxIssuesContext))]
    [Migration("20230820154732_jxIssueIID")]
    partial class jxIssueIID
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("libJxIssues.Models.LastSyncInfo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime?>("lastSync")
                        .HasColumnType("datetime");

                    b.HasKey("id");

                    b.ToTable("lastSyncInfo");
                });

            modelBuilder.Entity("libJxIssues.Models.Mitarbeiter", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("gitUsername")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("id");

                    b.ToTable("mitarbeiter");
                });

            modelBuilder.Entity("libJxIssues.Models.jxIssue", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime?>("deadline")
                        .HasColumnType("datetime");

                    b.Property<bool>("erledigt")
                        .HasColumnType("bit");

                    b.Property<bool>("git")
                        .HasColumnType("bit");

                    b.Property<int>("iid")
                        .HasColumnType("int");

                    b.Property<int?>("mitarbeiter_id")
                        .HasColumnType("int");

                    b.Property<string>("originallink")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("prioPunkte")
                        .HasColumnType("int");

                    b.Property<int?>("schaetzung")
                        .HasColumnType("int");

                    b.Property<int?>("sortorder")
                        .HasColumnType("int");

                    b.Property<DateTime?>("start")
                        .HasColumnType("datetime");

                    b.Property<int?>("status")
                        .HasColumnType("int");

                    b.Property<string>("titel")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.Property<int>("typ")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("jxIssue");
                });
#pragma warning restore 612, 618
        }
    }
}
