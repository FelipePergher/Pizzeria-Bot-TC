using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Data.Models.UserModels;
using Pizzaria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class BotUserState
    {

        public BotUserState()
        {
            Order = new Order();
            Address = new Address
            {
                AddressId = -1
            };
            Status = "";
            ReuseAddress = true;
        }

        public string Name { get; set; }

        public string Status { get; set; } 

        public Order Order { get; set; }

        public EntitiesState EntitiesState { get; set; }

        public Address Address { get; set; }

        public bool ReuseAddress { get; set; }

        public List<Address> UserAddresses { get; set; }

    }
}
