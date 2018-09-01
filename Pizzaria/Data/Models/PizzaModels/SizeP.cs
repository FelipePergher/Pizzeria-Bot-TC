using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.PizzaModels
{
    public class SizeP
    {
        public SizeP()
        {
            PizzaSizes = new HashSet<PizzaSize>();
        }

        [Key]
        public int SizePId { get; set; }

        public string Size { get; set; }

        public ICollection<PizzaSize> PizzaSizes { get; set; }

    }
}
