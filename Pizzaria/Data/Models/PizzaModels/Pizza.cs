using Pizzaria.Data.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.PizzaModels
{
    public class Pizza
    {
        [Key]
        public int PizzaId { get; set; }

        public bool Vegetarian { get; set; }

        public string Name { get; set; }

        public string PizzaType { get; set; }

        public string Image { get; set; }


        [NotMapped]
        public int UsedQuantity { get; set; }


        #region Many To many Relations

        public ICollection<PizzaIngredient> PizzaIngredients  { get; set; }

        public ICollection<PizzaSize> PizzaSizes { get; set; }

        public ICollection<OrderPizza> OrderPizzas { get; set; }

        #endregion
    }
}
