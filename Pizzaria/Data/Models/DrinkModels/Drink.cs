using Pizzaria.Data.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class Drink
    {
        [Key]
        public int DrinkId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public double Price { get; set; }


        [NotMapped]
        public int UsedQuantity { get; set; }


        #region Many To many Relations

        public ICollection<DrinkSize> DrinkSizes { get; set; }

        public ICollection<OrderDrink> OrderDrinks { get; set; }

        #endregion
    }
}
