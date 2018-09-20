using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class None : DialogSet
    {
        public const string NoneText = "None";

        #region Async Methods

        private async Task NoneBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            EntitiesParse entities = (EntitiesParse) args["entities"];
            //Todo: Usar as intenções para já continuar a conversa
            if(entities.Ingredients.Count > 0 || entities.Drinks.Count > 0)
            {
                await dialogContext.Context.SendActivity($"Me desculpe {dialogContext.Context.Activity.From.Name}, mas não consegui entender o que você gostaria :(");
                await dialogContext.Context.SendActivity("Mas baseado em informações encontradas na sua mensagem lhe recomendo os seguintes produtos :)");
                await dialogContext.Begin(AskProduct.Ask_Product_Waterfall_Text, args);
            }
            else
            {
                await dialogContext.Context.SendActivity($"Me desculpe {dialogContext.Context.Activity.From.Name}, mas não consegui entender o que você gostaria :(");
            }
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
