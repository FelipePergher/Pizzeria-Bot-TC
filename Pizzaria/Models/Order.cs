using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Models
{
    public class Order
    {
        public List<PizzaModel> Pizzas { get; set; }

        public List<DrinkModel> Drinks { get; set; }

        public int QuantityTotal { get; set; }

        public double PriceTotal { get; set; }
    }
}
