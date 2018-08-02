﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.PizzaModels
{
    public class Ingredient
    {
        public Ingredient()
        {
            PizzaIngredients = new HashSet<PizzaIngredient>();
        }
        [Key]
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public ICollection<PizzaIngredient> PizzaIngredients  { get; set; }
    }
}
