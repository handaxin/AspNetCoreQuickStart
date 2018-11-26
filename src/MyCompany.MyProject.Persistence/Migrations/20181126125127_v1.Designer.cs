﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCompany.MyProject.Persistence;

namespace MyCompany.MyProject.Persistence.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    [Migration("20181126125127_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyCompany.MyProject.Domain.DemoAggregate.Demo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Demo");
                });

            modelBuilder.Entity("MyCompany.MyProject.Domain.DemoAggregate.DemoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DemoId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(true);

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.HasIndex("DemoId");

                    b.ToTable("DemoItem");
                });

            modelBuilder.Entity("MyCompany.MyProject.Domain.DemoAggregate.DemoItem", b =>
                {
                    b.HasOne("MyCompany.MyProject.Domain.DemoAggregate.Demo")
                        .WithMany("DemoItems")
                        .HasForeignKey("DemoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
