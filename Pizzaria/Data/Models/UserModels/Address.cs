using System.ComponentModel.DataAnnotations;

namespace Pizzaria.Data.Models.UserModels
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public int Number { get; set; }
    }
}