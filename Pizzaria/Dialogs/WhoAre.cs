using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class WhoAre : DialogSet
    {
        public const string WhoAreWaterfallText = "Who_Are";

        private async Task WhoAreYou(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity($"Olá, eu sou o Jorge {Emojis.SmileHappy} o _bot_ da pizzaria do Manolo que estou aqui para auxiliá-lo em seu pedido\n" +
                $"Caso possua alguma duvida simplesmente digite *AJUDA* e lhe encaminharei para exemplos de utilização {Emojis.SmileHappy}\n" +
                $"Caso queira sair de alguma parte da conversa simplesmente digite *SAIR* e voltaremos ao fluxo normal da conversa {Emojis.SmileHappy}\n" +
                $"Em que lhe posso ser útil no momento?");
        }


        public WaterfallStep[] WhoAreWaterfall()
        {
            return new WaterfallStep[]
            {
                WhoAreYou
            };
        }
    }
}
