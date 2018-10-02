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
            OrderModel = new OrderModel();
            Address = new Address
            {
                AddressId = -1
            };
            Status = "";
            ReuseAddress = true;
            Skip = false;
            SkipAddress = false;
            Delivery = false;
            OrderEditIsEdit = false;
        }

        public string Name { get; set; }

        public string Status { get; set; } 

        public OrderModel OrderModel { get; set; }

        public EntitiesState EntitiesState { get; set; }

        public Address Address { get; set; }

        public bool ReuseAddress { get; set; }

        public List<Address> UserAddresses { get; set; }

        public bool Skip { get; set; }
        public bool SkipAddress { get; set; }

        public bool Delivery { get; set; }

        public int OrderIdEdit { get; set; }

        public string OrderSizeNameEdit { get; set; }

        public bool OrderEditIsPizza { get; set; }

        public bool OrderEditIsEdit { get; set; }

    }
}
