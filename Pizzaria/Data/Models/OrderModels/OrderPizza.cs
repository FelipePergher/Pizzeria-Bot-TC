using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.OrderModels
{
    public class OrderPizza
    {
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
