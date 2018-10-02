using Microsoft.EntityFrameworkCore;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.OrderModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ConversationData> ConversationDatas { get; set; }

        public DbSet<Drink> Drinks { get; set; }

        public DbSet<SizeD> SizesD { get; set; }

        public DbSet<SizeP> SizesP { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDrink> OrderDrinks { get; set; }

        public DbSet<OrderPizza> OrderPizzas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Many to Many Relations

            //Drink with Drink Size N to N
            modelBuilder.Entity<DrinkSize>()
            .HasKey(bc => new { bc.DrinkId, bc.SizeDId});

            modelBuilder.Entity<DrinkSize>()
                .HasOne(bc => bc.Drink)
                .WithMany(b => b.DrinkSizes)
                .HasForeignKey(bc => bc.DrinkId);

            modelBuilder.Entity<DrinkSize>()
                .HasOne(bc => bc.SizeD)
                .WithMany(c => c.DrinkSizes)
                .HasForeignKey(bc => bc.SizeDId);

            //Pizza with Pizza Size N to N
            modelBuilder.Entity<PizzaSize>()
            .HasKey(bc => new { bc.PizzaId, bc.SizePId });

            modelBuilder.Entity<PizzaSize>()
                .HasOne(bc => bc.Pizza)
                .WithMany(b => b.PizzaSizes)
                .HasForeignKey(bc => bc.PizzaId);

            modelBuilder.Entity<PizzaSize>()
                .HasOne(bc => bc.SizeP)
                .WithMany(c => c.PizzaSizes)
                .HasForeignKey(bc => bc.SizePId);

            //Ingredient with Pizza N to N
            modelBuilder.Entity<PizzaIngredient>()
           .HasKey(bc => new { bc.PizzaId, bc.IngredientId });

            modelBuilder.Entity<PizzaIngredient>()
                .HasOne(bc => bc.Ingredient)
                .WithMany(c => c.PizzaIngredients)
                .HasForeignKey(bc => bc.IngredientId);

            modelBuilder.Entity<PizzaIngredient>()
                .HasOne(bc => bc.Pizza)
                .WithMany(b => b.PizzaIngredients)
                .HasForeignKey(bc => bc.PizzaId);

            //OrderDrink with Order N to N
            modelBuilder.Entity<OrderDrink>()
           .HasKey(bc => new { bc.OrderId, bc.DrinkId });

            modelBuilder.Entity<OrderDrink>()
                .HasOne(bc => bc.Order)
                .WithMany(c => c.OrderDrinks)
                .HasForeignKey(bc => bc.OrderId);

            modelBuilder.Entity<OrderDrink>()
                .HasOne(bc => bc.Drink)
                .WithMany(b => b.OrderDrinks)
                .HasForeignKey(bc => bc.DrinkId);

            //OrderPizza with Order N to N
            modelBuilder.Entity<OrderPizza>()
           .HasKey(bc => new { bc.OrderId, bc.PizzaId });

            modelBuilder.Entity<OrderPizza>()
                .HasOne(bc => bc.Order)
                .WithMany(c => c.OrderPizzas)
                .HasForeignKey(bc => bc.OrderId);

            modelBuilder.Entity<OrderPizza>()
                .HasOne(bc => bc.Pizza)
                .WithMany(b => b.OrderPizzas)
                .HasForeignKey(bc => bc.PizzaId);

            #endregion
        }
    }
}
