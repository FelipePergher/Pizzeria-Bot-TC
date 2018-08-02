using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class DrinkSize
    {
        public int SizeId { get; set; }

        public Size Size { get; set; }

        public int DrinkId { get; set; }

        public Drink Drink { get; set; }
    }
}
