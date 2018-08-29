using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class EntitiesParse
    {
        public EntitiesParse()
        {
            Drinks = new List<string>();
            Ingredients = new List<string>();
        }

        public List<string> Drinks { get; set; }
        public List<string> Ingredients { get; set; }

        public static EntitiesParse RecognizeEntities(JObject jObject)
        {
            EntitiesParse entitiesParse = new EntitiesParse();
            var entities = new List<string>();

            foreach (var entity in jObject)
            {
                if (!entity.Key.ToString().Equals("$instance"))
                {
                    foreach (var item in entity.Value)
                    {
                        if(entity.Key == "Drink")
                        {
                            entitiesParse.Drinks.Add(item.First.ToString());
                        } else if(entity.Key == "Ingredients")
                        {
                            entitiesParse.Ingredients.Add(item.First.ToString());
                        }
                    }
                    
                }
            }

            return (entitiesParse);
        }
    }
}
