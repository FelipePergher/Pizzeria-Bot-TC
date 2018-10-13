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
            await dialogContext.Context.SendActivity($"Eu sou o Jorge {Emojis.ManRaisingHand} o bot da Pergher pizzaria. Estou aqui para te ajudar em seu pedido.  \n" +
                           "Eu posso realizar as seguintes tarefas quando solicitado:  \n" +
                           "* Ofereço bebidas e pizzas customizadas  \n" +
                           "* Mostro como seu carrinho está no momento  \n" +
                           "* Limpo seu carrinho  \n" +
                           "* Finalizo seu carrinho  \n" +
                           "* Edito e removo itens do seu carrinho  \n" +
                           "* Edito seu endereço de entrega atual  \n" +
                           "* Busco seus pedidos abertos para saber você acompanhar  \n");

            await dialogContext.Context.SendActivity($"Dúvidas? Digite *AJUDA* e será encaminhado ao manual de utilização {Emojis.SmileHappy}  \n" +
                              $"Problemas em alguma parte da conversa? Digite *SAIR* e voltaremos ao fluxo normal {Emojis.SmileHappy}");
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
