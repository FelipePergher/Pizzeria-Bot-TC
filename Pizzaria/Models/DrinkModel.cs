using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Models
{
    public class DrinkModel
    {
        public int DrinkId { get; set; }

        public string DrinkName { get; set; }

        public string DrinkSizeName { get; set; }

        public double DrinkQuantity { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
