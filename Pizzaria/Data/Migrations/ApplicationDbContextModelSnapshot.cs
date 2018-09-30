﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pizzaria.Data.Models;

namespace Pizzaria.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview1-35029")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Pizzaria.Data.Models.DrinkModels.Drink", b =>
                {
                    b.Property<int>("DrinkId")
                        .ValueGeneratedOnAdd();

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
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Quantity");

                    b.Property<string>("SizeName");

                    b.HasKey("SizeDId");

                    b.ToTable("SizesD");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AmmountTotal");

                    b.Property<bool>("Delivery");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<int?>("UsedAddressAddressId");

                    b.Property<int?>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("UsedAddressAddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderDrink", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("DrinkId");

                    b.Property<string>("DrinkSizeName");

                    b.HasKey("OrderId", "DrinkId");

                    b.HasIndex("DrinkId");

                    b.ToTable("OrderDrinks");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderDrinkSize", b =>
                {
                    b.Property<int>("OrderDrinkSizeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DrinkSizeSizeDId");

                    b.Property<int?>("OrderDrinkDrinkId");

                    b.Property<int?>("OrderDrinkOrderId");

                    b.Property<double>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderDrinkSizeId");

                    b.HasIndex("DrinkSizeSizeDId");

                    b.HasIndex("OrderDrinkOrderId", "OrderDrinkDrinkId");

                    b.ToTable("OrderDrinkSize");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderPizza", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("PizzaId");

                    b.Property<string>("PizzaSizeName");

                    b.HasKey("OrderId", "PizzaId");

                    b.HasIndex("PizzaId");

                    b.ToTable("OrderPizzas");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderPizzaSize", b =>
                {
                    b.Property<int>("OrderPizzaSizeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OrderPizzaOrderId");

                    b.Property<int?>("OrderPizzaPizzaId");

                    b.Property<int?>("PizzaSizeSizePId");

                    b.Property<double>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("OrderPizzaSizeId");

                    b.HasIndex("PizzaSizeSizePId");

                    b.HasIndex("OrderPizzaOrderId", "OrderPizzaPizzaId");

                    b.ToTable("OrderPizzaSize");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("IngredientId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.Pizza", b =>
                {
                    b.Property<int>("PizzaId")
                        .ValueGeneratedOnAdd();

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

                    b.Property<double>("Price");

                    b.HasKey("PizzaId", "SizePId");

                    b.HasIndex("SizePId");

                    b.ToTable("PizzaSize");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.PizzaModels.SizeP", b =>
                {
                    b.Property<int>("SizePId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Size");

                    b.HasKey("SizePId");

                    b.ToTable("SizesP");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Neighborhood");

                    b.Property<int>("Number");

                    b.Property<string>("Street");

                    b.Property<int>("UserId");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.ConversationData", b =>
                {
                    b.Property<int>("ConversationDataId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ConversationDataId");

                    b.ToTable("ConversationDatas");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.UserModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConversationDataId");

                    b.Property<string>("Name");

                    b.Property<string>("UserIdBot");

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

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.Order", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.UserModels.Address", "UsedAddress")
                        .WithMany()
                        .HasForeignKey("UsedAddressAddressId");

                    b.HasOne("Pizzaria.Data.Models.UserModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderDrink", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.DrinkModels.Drink", "Drink")
                        .WithMany("OrderDrinks")
                        .HasForeignKey("DrinkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pizzaria.Data.Models.OrderModels.Order", "Order")
                        .WithMany("OrderDrinks")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderDrinkSize", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.DrinkModels.SizeD", "DrinkSize")
                        .WithMany()
                        .HasForeignKey("DrinkSizeSizeDId");

                    b.HasOne("Pizzaria.Data.Models.OrderModels.OrderDrink")
                        .WithMany("OrderDrinkSizes")
                        .HasForeignKey("OrderDrinkOrderId", "OrderDrinkDrinkId");
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderPizza", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.OrderModels.Order", "Order")
                        .WithMany("OrderPizzas")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pizzaria.Data.Models.PizzaModels.Pizza", "Pizza")
                        .WithMany("OrderPizzas")
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pizzaria.Data.Models.OrderModels.OrderPizzaSize", b =>
                {
                    b.HasOne("Pizzaria.Data.Models.PizzaModels.SizeP", "PizzaSize")
                        .WithMany()
                        .HasForeignKey("PizzaSizeSizePId");

                    b.HasOne("Pizzaria.Data.Models.OrderModels.OrderPizza")
                        .WithMany("OrderPizzaSizes")
                        .HasForeignKey("OrderPizzaOrderId", "OrderPizzaPizzaId");
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
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
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
