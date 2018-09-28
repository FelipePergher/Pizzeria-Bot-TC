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

            //todo: pegar as mais vendidas primeiro
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

        public static List<Pizza> GetPizzasMoreSalesWithoutByIngredients(ApplicationDbContext context, List<Pizza> pizzasByIngredients)
        {
            List<Pizza> pizzas = new List<Pizza>();
            //Todo: Ordenar baseado na quantidade de vendas
            //context.Pizzas.OrderBy(x => x.)

            pizzas = pizzas.Except(pizzasByIngredients).ToList();

            return pizzas;
        }

        public static List<Pizza> GetPizzasMoreSales(ApplicationDbContext context)
        {
            List<Pizza> pizzas = context.Pizzas
                    .Include(x => x.PizzaIngredients)
                        .ThenInclude(y => y.Ingredient)
                    .Include(x => x.PizzaSizes).
                        ThenInclude(y => y.SizeP).ToList();
            //Todo: Ordenar baseado na quantidade de vendas
            //context.Pizzas.OrderBy(x => x.)

            return pizzas;
        }

        public static List<Drink> GetDrinksMoreSalesWithUserDrinks(List<string> drinksFind, ApplicationDbContext context)
        {
            //Todo: Ordenar baseado na quantidade de vendas
            //Todo: colocar os que o usuário solicitou primeiro
            List<Drink> drinks = context.Drinks
                    .Include(x => x.DrinkSizes)
                        .ThenInclude(y => y.SizeD).ToList();
            return drinks.OrderBy(x => x.Name).ToList();
        }

        public static List<Drink> GetDrinksMoreSales(ApplicationDbContext context)
        {
            //Todo: Ordenar baseado na quantidade de vendas
            List<Drink> drinks = context.Drinks
                    .Include(x => x.DrinkSizes)
                        .ThenInclude(y => y.SizeD).ToList();
            return drinks.OrderBy(x => x.Name).ToList();
        }

    }

    public class PizzaRecomendation
    {
        public Pizza Pizza { get; set; }

        public int IngredientsQuantity { get; set; }
    }

}
