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
            await dialogContext.Context.SendActivity($"Eu sou o Jorge o bot da pizzaria do manolo e estou aqui para auxiliá-lo no seu pedido {Emojis.SmileHappy} \n" +
                           "Atualmente eu posso realizar as seguintes tarefas: \n" +
                           "*-Ofereço bebidas e pizzas cutomizadas na sua solicitação* \n" +
                           "*-Mostro como seu carrinho está no momento* \n" +
                           "*-Limpo seu carrinho quando solicitado* \n" +
                           "*-Finalizo seu carrinho quando solicitado* \n" +
                           "*-Edito e removo itens seu carrinho quando solicitado* \n" +
                           "*-Edito seu endreço de entrega atual quando solicitado* \n" +
                           "*-Busco seus pedidos abertos para saber o seu estado* \n");

            await dialogContext.Context.SendActivity($"Caso possua alguma duvida simplesmente digite *AJUDA* e lhe encaminharei para exemplos de utilização {Emojis.SmileHappy}\n" +
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
