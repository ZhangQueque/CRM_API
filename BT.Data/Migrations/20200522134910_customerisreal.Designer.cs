﻿// <auto-generated />
using System;
using BT.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BT.Data.Migrations
{
    [DbContext(typeof(CRMContext))]
    [Migration("20200522134910_customerisreal")]
    partial class customerisreal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BT.Data.Entities.Customers", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Alias")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<int>("IsReal")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BT.Data.Entities.Employees", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("EmployeePID")
                        .HasColumnType("int");

                    b.Property<string>("HeadImage")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("BT.Data.Entities.Employees_Roles", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Employees_Roles");
                });

            modelBuilder.Entity("BT.Data.Entities.LogCategories", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("LogCategories");
                });

            modelBuilder.Entity("BT.Data.Entities.Logs", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("BT.Data.Entities.Permissions", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActiveName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<int>("Leave")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PID")
                        .HasColumnType("int");

                    b.Property<string>("PathCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ShortDescribe")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("BT.Data.Entities.PublicCounts", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("PublicCounts");
                });

            modelBuilder.Entity("BT.Data.Entities.Roles", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BT.Data.Entities.Roles_Permissions", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<int>("PermissionID")
                        .HasColumnType("int");

                    b.Property<string>("PermissionName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("Roles_Permissions");
                });

            modelBuilder.Entity("BT.Data.Entities.TrackRecords", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("IsDel")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("ID");

                    b.ToTable("TrackRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
