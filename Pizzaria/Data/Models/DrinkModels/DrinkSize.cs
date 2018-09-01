using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class DrinkSize
    {
        public int SizeDId { get; set; }

        public SizeD SizeD { get; set; }

        public int DrinkId { get; set; }

        public Drink Drink { get; set; }

        public double Price { get; set; }
    }
}
