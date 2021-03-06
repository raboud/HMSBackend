﻿// <auto-generated />
using HMS.Catalog.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Catalog.API.Migrations.Catalog
{
    [DbContext(typeof(CatalogContext))]
    partial class CatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("Relational:Sequence:.catalog_brand_hilo", "'catalog_brand_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.catalog_category_hilo", "'catalog_category_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.catalog_product_hilo", "'catalog_product_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HMS.Catalog.API.Model.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_brand_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<bool>("InActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_category_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<bool>("InActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "catalog_product_hilo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<int>("AvailableStock");

                    b.Property<int>("BrandId");

                    b.Property<decimal>("Cost");

                    b.Property<int>("Count");

                    b.Property<string>("Description");

                    b.Property<bool>("InActive");

                    b.Property<int>("MaxStockThreshold");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("OnReorder");

                    b.Property<string>("PictureFileName");

                    b.Property<decimal>("Price");

                    b.Property<int>("RestockThreshold");

                    b.Property<decimal>("SuggestPrice");

                    b.Property<int>("UnitId");

                    b.Property<int>("VendorId");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("UnitId");

                    b.HasIndex("VendorId");

                    b.HasIndex("Name", "Count", "UnitId")
                        .IsUnique();

                    b.ToTable("Product");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.ProductCategory", b =>
                {
                    b.Property<int>("ItemId");

                    b.Property<int>("CategoryId");

                    b.HasKey("ItemId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("InActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("InActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.Product", b =>
                {
                    b.HasOne("HMS.Catalog.API.Model.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HMS.Catalog.API.Model.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HMS.Catalog.API.Model.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HMS.Catalog.API.Model.ProductCategory", b =>
                {
                    b.HasOne("HMS.Catalog.API.Model.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HMS.Catalog.API.Model.Product", "Item")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
