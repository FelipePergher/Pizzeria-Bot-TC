using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.PizzaModels
{
    public class PizzaSize
    {
        public int PizzaSizeId { get; set; }

        public int SizePId { get; set; }

        public SizeP SizeP { get; set; }

        public int PizzaId { get; set; }

        public Pizza Pizza { get; set; }

        public double Price { get; set; }
    }
}
