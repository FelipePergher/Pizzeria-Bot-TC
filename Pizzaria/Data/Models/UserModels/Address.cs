using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzaria.Data.Models.UserModels
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public string Number { get; set; }

        public int UserId { get; set; }

        [NotMapped]
        public DateTime LastUsedDate { get; set; }
    }
}