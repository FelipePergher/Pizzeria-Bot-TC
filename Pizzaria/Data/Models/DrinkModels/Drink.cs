using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class Drink
    {
        public Drink()
        {
            DrinkSizes = new HashSet<DrinkSize>();
        }

        [Key]
        public int DrinkId { get; set; }

        public string Name { get; set; }

        public ICollection<DrinkSize> DrinkSizes { get; set; }
    }       
}
