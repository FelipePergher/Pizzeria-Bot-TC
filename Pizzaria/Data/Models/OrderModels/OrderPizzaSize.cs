using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.OrderModels
{
    public class OrderPizzaSize
    {
        public int OrderPizzaSizeId { get; set; }

        public SizeP PizzaSize { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
