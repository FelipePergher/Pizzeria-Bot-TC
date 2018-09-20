using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Models
{
    public class Order
    {
        public Order()
        {
            Pizzas = new List<PizzaModel>();
            Drinks = new List<DrinkModel>();
            PriceTotal = 0;
            QuantityTotal = 0;
        }

        public List<PizzaModel> Pizzas { get; set; }

        public List<DrinkModel> Drinks { get; set; }

        public int QuantityTotal { get; set; }

        public double PriceTotal { get; set; }
    }
}
