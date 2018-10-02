using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.OrderModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime RegisterDate { get; set; }

        public User User { get; set; }

        public double AmmountTotal { get; set; }

        public Address UsedAddress { get; set; }

        public bool Delivery { get; set; }

        public bool Finished { get; set; }

        public string OrderStatus { get; set; }

        #region Many To Many 

        public ICollection<OrderDrink> OrderDrinks { get; set; }

        public ICollection<OrderPizza> OrderPizzas { get; set; }

        #endregion
    }
}
