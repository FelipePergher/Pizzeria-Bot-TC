using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class BotUserState
    {

        public BotUserState()
        {
            Order = new Order();
            Status = "";
        }

        public string Name { get; set; }

        //Todo: Usado para pular o comprimento do usuário
        public string Status { get; set; } 

        public Order Order { get; set; }

        public EntitiesState EntitiesState { get; set; }

    }
}
