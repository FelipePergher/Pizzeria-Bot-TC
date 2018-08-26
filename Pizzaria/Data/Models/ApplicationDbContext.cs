using Microsoft.EntityFrameworkCore;
using Pizzaria.Data.Models.DrinkModels;
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

        public DbSet<Size> Sizes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DrinkSize>()
            .HasKey(bc => new { bc.DrinkId, bc.SizeId});

            modelBuilder.Entity<DrinkSize>()
                .HasOne(bc => bc.Drink)
                .WithMany(b => b.DrinkSizes)
                .HasForeignKey(bc => bc.DrinkId);

            modelBuilder.Entity<DrinkSize>()
                .HasOne(bc => bc.Size)
                .WithMany(c => c.DrinkSizes)
                .HasForeignKey(bc => bc.SizeId);

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
        }
    }
}
