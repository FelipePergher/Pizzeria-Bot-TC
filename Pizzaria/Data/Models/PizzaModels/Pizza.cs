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
            PizzaIngredients = new HashSet<PizzaIngredient>();
            PizzaSizes = new HashSet<PizzaSize>();
        }

        [Key]
        public int PizzaId { get; set; }

        public bool Vegetarian { get; set; }

        public string Name { get; set; }

        public string PizzaType { get; set; }

        public string Image { get; set; }

        public ICollection<PizzaIngredient> PizzaIngredients  { get; set; }
        public ICollection<PizzaSize> PizzaSizes { get; set; }
    }
}
