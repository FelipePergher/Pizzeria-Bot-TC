using Microsoft.EntityFrameworkCore;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class EntitieRecomendation
    {
        public static List<Pizza> GetPizzasByIngredients(List<string> ingredients, ApplicationDbContext context)
        {
            List<Pizza> pizzas = new List<Pizza>();
            foreach (var ingredient in ingredients)
            {
                List<Pizza> pizzaFind = context.Pizzas
                    .Include(x => x.PizzaIngredients)
                        .ThenInclude(y => y.Ingredient)
                    .Include(x => x.PizzaSizes).
                        ThenInclude(y => y.SizeP)
                    .Where(x => x.PizzaIngredients.Where(y => y.Ingredient.Name.ToLower() == ingredient).Count() > 0).ToList();
                pizzas.AddRange(pizzaFind);
            }

            List<PizzaRecomendation> pizzaRecomendations = new List<PizzaRecomendation>();

            List<Pizza> allpizzas = context.Pizzas
                    .Include(x => x.PizzaIngredients)
                        .ThenInclude(y => y.Ingredient)
                    .Include(x => x.PizzaSizes).
                        ThenInclude(y => y.SizeP).ToList();

            foreach (var pizza in allpizzas)
            {
                PizzaRecomendation pizzaRecomendation = pizzaRecomendations.Where(x => x.Pizza == pizza).FirstOrDefault();
                if (pizzaRecomendation == null)
                {
                    pizza.PizzaSizes = pizza.PizzaSizes.OrderBy(x => x.Price).ToList();
                    pizzaRecomendations.Add(new PizzaRecomendation
                    {
                        Pizza = pizza,
                        IngredientsQuantity = 0
                    });
                }
                else
                {
                    pizzaRecomendations.Where(x => x.Pizza == pizza).FirstOrDefault().IngredientsQuantity++;
                }

            }

            return pizzaRecomendations.OrderBy(x => x.IngredientsQuantity).Select(x => x.Pizza).ToList();
        }

        public static List<Pizza> GetPizzasMoreSales(ApplicationDbContext context)
        {
            List<Pizza> pizzas = context.Pizzas
                    .Include(x => x.PizzaIngredients)
                        .ThenInclude(y => y.Ingredient)
                    .Include(x => x.PizzaSizes).
                        ThenInclude(y => y.SizeP).ToList();

            foreach (var pizza in pizzas)
            {
                pizza.UsedQuantity = context.OrderPizzas.Where(x => x.PizzaId == pizza.PizzaId).Count();
            }

            return pizzas.OrderBy(x => x.UsedQuantity).ToList();
        }

        public static List<Drink> GetDrinksMoreSalesWithUserDrinks(List<string> drinksFind, ApplicationDbContext context)
        {
            List<Drink> drinksFinded = new List<Drink>();
            foreach (var item in drinksFind)
            {
                Drink drink = context.Drinks.FirstOrDefault(x => x.Name == item);
                drinksFinded.Add(drink);
            }

            List<Drink> drinks = context.Drinks
                    .Include(x => x.DrinkSizes)
                        .ThenInclude(y => y.SizeD).ToList();
            drinks.Except(drinksFinded);

            foreach (var drink in drinks)
            {
                drink.UsedQuantity = context.OrderDrinks.Where(x => x.DrinkId == drink.DrinkId).Count();
            }

            drinks.OrderBy(x => x.UsedQuantity);
            drinksFinded.AddRange(drinks);

            return drinksFinded;
        }

        public static List<Drink> GetDrinksMoreSales(ApplicationDbContext context)
        {
            List<Drink> drinks = context.Drinks
                    .Include(x => x.DrinkSizes)
                        .ThenInclude(y => y.SizeD).ToList();

            foreach (var drink in drinks)
            {
                drink.UsedQuantity = context.OrderDrinks.Where(x => x.DrinkId == drink.DrinkId).Count();
            }

            return drinks.OrderBy(x => x.UsedQuantity).ToList();
        }

    }

    public class PizzaRecomendation
    {
        public Pizza Pizza { get; set; }

        public int IngredientsQuantity { get; set; }
    }

}
