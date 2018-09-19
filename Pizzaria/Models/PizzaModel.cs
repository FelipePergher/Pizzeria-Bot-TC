using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Models
{
    public class PizzaModel
    {
        public int PizzaId { get; set; }

        public string PizzaName { get; set; }

        public int Quantity { get; set; }

        public string PizzaSize { get; set; }

        public int PizzaSizeId { get; set; }

        public double Price { get; set; }
    }
}
