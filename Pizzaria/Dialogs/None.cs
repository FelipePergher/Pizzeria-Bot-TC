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

        private Task NoneBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            EntitiesParse entities = (EntitiesParse) args["entities"];
            //Todo: Usar as intenções para já continuar a conversa
            return dialogContext.Context.SendActivity("Me desculpe, mas não consegui entender o que você gostaria :(");
        }

        public WaterfallStep[] NoneWaterfall()
        {
            return new WaterfallStep[]
            {
                NoneBegin
            };
        }
    }
}
