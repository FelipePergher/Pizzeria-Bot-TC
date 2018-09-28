using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class None : DialogSet
    {
        public const string NoneText = "None";
        private readonly ApplicationDbContext context;

        public None()
        {
            context = ServiceProviderFactory.GetApplicationDbContext();
        }

        #region Async Methods

        private async Task NoneBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            EntitiesParse entities = (EntitiesParse) args["entities"];

            if (entities.Ingredients.Count > 0 || entities.Drinks.Count > 0 || entities.ProductTypes.Count > 0)
            {
                await dialogContext.Context.SendActivity($"Me desculpe { dialogContext.Context.Activity.From.Name}, mas não consegui entender o que você gostaria :( " +
                    $"\nMas baseado em informações encontradas na sua mensagem lhe recomendo os seguintes produtos :)");
            }
            else
            {
                await dialogContext.Context.SendActivity($"Me desculpe { dialogContext.Context.Activity.From.Name}, mas não consegui entender o que você gostaria :( " +
                    $"\nMas estou enviando algumas pizzas para você ver :)");
            }
            await dialogContext.Begin(AskProduct.Ask_Product_Waterfall_Text, args);

        }

        #endregion

        #region Waterfall

        public WaterfallStep[] NoneWaterfall()
        {
            return new WaterfallStep[]
            {
                NoneBegin
            };
        }
        
        #endregion
    }
}
