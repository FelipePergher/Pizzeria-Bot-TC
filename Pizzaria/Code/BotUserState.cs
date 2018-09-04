using Pizzaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class BotUserState
    {
        public string Name { get; set; }

        public string Status { get; set; } = "a";

        public Order Order { get; set; }
    }
}
