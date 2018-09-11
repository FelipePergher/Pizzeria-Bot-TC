﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pizzaria.Data.Models;

namespace Pizzaria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180901184130_PriceSizePizza")]
    partial class PriceSizePizza
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview1-35029")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Pizzaria.Data.Models.DrinkModels.Drink", b =>
                {
                    b.Property<int>("DrinkId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.HasKey("DrinkId");

                    b.ToTable("Drinks");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.DrinkModels.DrinkSize", b =>
                {
                    b.Property<int>("DrinkId");

                    b.Property<int>("SizeDId");

                    b.Property<double>("Price");

                    b.HasKey("DrinkId", "SizeDId");

                    b.HasIndex("SizeDId");

                    b.ToTable("DrinkSize");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.DrinkModels.SizeD", b =>
                {
                    b.Property<int>("SizeDId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Quantity");

                    b.HasKey("SizeDId");

                    b.ToTable("SizesD");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("IngredientId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.Pizza", b =>
                {
                    b.Property<int>("PizzaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<string>("PizzaType");

                    b.Property<bool>("Vegetarian");

                    b.HasKey("PizzaId");

                    b.ToTable("Pizzas");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.PizzaIngredient", b =>
                {
                    b.Property<int>("PizzaId");

                    b.Property<int>("IngredientId");

                    b.HasKey("PizzaId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("PizzaIngredient");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.PizzaSize", b =>
                {
                    b.Property<int>("PizzaId");

                    b.Property<int>("SizePId");

                    b.Property<int>("PizzaSizeId");

                    b.Property<double>("Price");

                    b.HasKey("PizzaId", "SizePId");

                    b.HasIndex("SizePId");

                    b.ToTable("PizzaSize");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.SizeP", b =>
                {
                    b.Property<int>("SizePId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Size");

                    b.HasKey("SizePId");

                    b.ToTable("SizesP");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Neighborhood");

                    b.Property<int>("Number");

                    b.Property<string>("Street");

                    b.Property<int?>("UserId");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.ConversationData", b =>
                {
                    b.Property<int>("ConversationDataId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("ConversationDataId");

                    b.ToTable("ConversationDatas");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ConversationDataId");

                    b.HasKey("UserId");

                    b.HasIndex("ConversationDataId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.DrinkModels.DrinkSize", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.DrinkModels.Drink", "Drink")
                        .WithMany("DrinkSizes")
                        .HasForeignKey("DrinkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pizzaria.Data.Models.DrinkModels.SizeD", "SizeD")
                        .WithMany("DrinkSizes")
                        .HasForeignKey("SizeDId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.PizzaIngredient", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.PizzaModels.Ingredient", "Ingredient")
                        .WithMany("PizzaIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pizzaria.Data.Models.PizzaModels.Pizza", "Pizza")
                        .WithMany("PizzaIngredients")
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.PizzaSize", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.PizzaModels.Pizza", "Pizza")
                        .WithMany("PizzaSizes")
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pizzaria.Data.Models.PizzaModels.SizeP", "SizeP")
                        .WithMany("PizzaSizes")
                        .HasForeignKey("SizePId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.Address", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.UserModels.User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.User", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.UserModels.ConversationData", "ConversationData")
                        .WithMany()
                        .HasForeignKey("ConversationDataId");
                });
#pragma warning restore 612, 618
        }
    }
}