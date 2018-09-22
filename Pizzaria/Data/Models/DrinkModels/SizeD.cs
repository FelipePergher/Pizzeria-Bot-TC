using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class SizeD
    {
        public SizeD()
        {
            DrinkSizes = new HashSet<DrinkSize>();
        }

        [Key]
        public int SizeDId { get; set; }

        public double Quantity { get; set; }

        public string SizeName { get; set; }

        public ICollection<DrinkSize> DrinkSizes { get; set; }

    }
}
