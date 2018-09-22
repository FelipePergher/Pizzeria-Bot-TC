using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class InformationName
    {
        //Pizza Types
        public static string Candy { get; } = "Doce";
        public static string Salted { get; } = "Salgada";

        //Pizza Sizes
        public static string Large { get; } = "Grande";
        public static string Family { get; } = "Família";
        public static string Medium { get; } = "Média";
        public static string Small { get; } = "Pequena";

        //Pizza Prices
        public static double LargePrice { get; } = 60d;
        public static double FamilyPrice { get; } = 50d;
        public static double MediumPrice { get; } = 40d;
        public static double SmallPrice { get; } = 25d;

        //Drink Sizes
        public static double Lata350 { get; set; } = 350d;
        public static double Garrafa500 { get; set; } = 500d;
        public static double Garrafa600 { get; set; } = 600;
        public static double Litro01 { get; set; } = 1d;
        public static double Litro15 { get; set; } = 1.5d;
        public static double Litro02 { get; set; } = 2d;

        //Drink Sizes
        public static string Lata350Name { get; set; } = "350 ml";
        public static string Garrafa500Name { get; set; } = "500 ml";
        public static string Garrafa600Name { get; set; } = "600 ml";
        public static string Litro01Name { get; set; } = "1 Ltr";
        public static string Litro15Name { get; set; } = "1.5 Ltr";
        public static string Litro02Name { get; set; } = "2 Ltr";

        //Entities
        public static string IngredientsEntitie { get; set; } = "Ingredients";
        public static string Product_TypeEntitie { get; set; } = "Product_Type";
        public static string Pizza_NameEntitie { get; set; } = "Pizza_Name";
        public static string DrinkEntitie { get; set; } = "Drink";

        //Product Type
        public string Drink { get; set; } = "bebida";
        public string Pizza { get; set; } = "pizza";
    }
}
