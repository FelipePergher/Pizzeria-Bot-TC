using Pizzaria.Code;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using System;

namespace Pizzaria.Data.Models
{
    public class EntitiesState
    {
        public DateTime AddedDate{ get; set; }

        public EntitiesParse EntitiesParse { get; set; }

        public int PizzasQuantityUsed { get; set; }

        public int DrinksQuantityUsed { get; set; }
    }
}