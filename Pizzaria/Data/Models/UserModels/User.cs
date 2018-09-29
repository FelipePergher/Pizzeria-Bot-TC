using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Data.Models.UserModels
{
    public class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
        }

        [Key]
        public int UserId { get; set; }

        public string UserIdBot { get; set; }

        public string Name { get; set; }

        public ConversationData ConversationData { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
