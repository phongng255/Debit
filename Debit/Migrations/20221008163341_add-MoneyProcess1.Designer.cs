﻿// <auto-generated />
using System;
using Debit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Debit.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221008163341_add-MoneyProcess1")]
    partial class addMoneyProcess1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Debit.Models.Accumulate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DebitId")
                        .HasColumnType("char(36)");

                    b.Property<decimal?>("Money")
                        .IsRequired()
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("DebitId");

                    b.ToTable("Accumulates");
                });

            modelBuilder.Entity("Debit.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Debit.Models.DebitCustomer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DateComplete")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Items")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Money")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("ProcessMoney")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("DebitCustomer");
                });

            modelBuilder.Entity("Debit.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PassworhHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Debit.Models.Accumulate", b =>
                {
                    b.HasOne("Debit.Models.DebitCustomer", "Debit")
                        .WithMany("Accumulates")
                        .HasForeignKey("DebitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Debit");
                });

            modelBuilder.Entity("Debit.Models.DebitCustomer", b =>
                {
                    b.HasOne("Debit.Models.Customer", "Customer")
                        .WithMany("Debits")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Debit.Models.Customer", b =>
                {
                    b.Navigation("Debits");
                });

            modelBuilder.Entity("Debit.Models.DebitCustomer", b =>
                {
                    b.Navigation("Accumulates");
                });
#pragma warning restore 612, 618
        }
    }
}
