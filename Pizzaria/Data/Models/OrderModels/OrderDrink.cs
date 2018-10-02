using Pizzaria.Data.Models.DrinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.OrderModels
{
    public class OrderDrink
    {
        public int DrinkId { get; set; }
        public Drink Drink { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public List<OrderDrinkSize>  OrderDrinkSizes { get; set; }

        public string DrinkSizeName { get; set; }
    }
}
