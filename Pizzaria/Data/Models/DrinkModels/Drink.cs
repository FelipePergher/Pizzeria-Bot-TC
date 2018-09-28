using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.DrinkModels
{
    public class Drink
    {
        [Key]
        public int DrinkId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public double Price { get; set; }


        #region Many To many Relations

        public ICollection<DrinkSize> DrinkSizes { get; set; }

        #endregion
    }
}
