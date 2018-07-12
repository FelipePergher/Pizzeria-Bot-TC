using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.PizzaModels
{
    public class Pizza
    {
        public Pizza()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        [Key]
        public int PizzaId { get; set; }

        public bool Vegetarian { get; set; }

        public ICollection<Ingredient> Ingredients  { get; set; }
    }
}
