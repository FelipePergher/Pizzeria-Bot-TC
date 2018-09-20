using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Models
{
    public class PizzaModel
    {
        public string PizzaName{ get; set; }

        public int PizzaId{ get; set; }

        public int PizzaSizeId { get; set; }

        public string SizeName { get; set; }

        public int SizeId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
