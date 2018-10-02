using Pizzaria.Data.Models.DrinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.OrderModels
{
    public class OrderDrinkSize
    {
        public int OrderDrinkSizeId { get; set; }

        public SizeD DrinkSize { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
