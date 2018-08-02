using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class Size
    {
        public Size()
        {
            DrinkSizes = new HashSet<DrinkSize>();
        }

        [Key]
        public int SizeId { get; set; }

        public double Quantity { get; set; }

        public ICollection<DrinkSize> DrinkSizes { get; set; }

    }
}
