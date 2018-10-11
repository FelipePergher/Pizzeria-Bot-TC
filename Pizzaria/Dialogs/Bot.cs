using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using System.Linq;
using Pizzaria.Code;
using Microsoft.Bot.Builder.Ai.LUIS;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using System;

namespace Pizzaria.Dialogs
{
    public class Bot : IBot
    {
        private const double LUIS_INTENT_THRESHOLD = 0.75d;
        private DialogFlow DialogFlow;

        public Bot()
        {
            DialogFlow = new DialogFlow();
        }

        public async Task OnTurn(ITurnContext turnContext)
        {
            /*if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate && turnContext.Activity.MembersAdded.FirstOrDefault()?.Id == turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivity($"Seja bem vindo a pizzaria do Manolo {Emojis.SmileHappy}");

                await turnContext.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });

                await turnContext.SendActivity($"Eu sou o Jorge o bot da pizzaria do manolo e estou aqui para auxiliá-lo no seu pedido {Emojis.SmileHappy} \n" +
                    "Atualmente eu posso realizar as seguintes tarefas: \n" +
                    "*-Ofereço bebidas e pizzas cutomizadas na sua solicitação* \n" +
                    "*-Mostro como seu carrinho está no momento* \n" +
                    "*-Limpo seu carrinho quando solicitado* \n" +
                    "*-Finalizo seu carrinho quando solicitado* \n" +
                    "*-Edito e removo itens seu carrinho quando solicitado* \n" +
                    "*-Edito seu endreço de entrega atual quando solicitado* \n" +
                    "*-Busco seus pedidos abertos para saber o seu estado* \n");

                await turnContext.SendActivity($"Quando tiver alguma dúvida simplesmente escreva *AJUDA* e lhe redirecionarei para exemplos de uso {Emojis.SmileHappy}\n" +
                    $"Caso queira sair de uma conversa que esteja no momento, simplesmente digite *SAIR* e voltaremos ao fluxo normal da conversa {Emojis.SmileHappy}\n" +
                    $"Em que lhe posso ser útil no momento?");

            }*/

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogState = turnContext.GetConversationState<Dictionary<string, object>>();
                DialogContext dialogContext = DialogFlow.CreateContext(turnContext, dialogState);

                if (turnContext.Activity.Text.ToLower() == "sair")
                {
                    dialogContext.EndAll();
                }
                else if (turnContext.Activity.Text.ToLower() == "ajuda")
                {
                    //IActivity activity = MessageFactory.SuggestedActions(new CardAction[]
                    //    {
                    //        new CardAction
                    //        {
                    //            Title = "Abrir documentação",
                    //            Type = ActionTypes.OpenUrl,
                    //            Value = "https://pizzeria-bot-tc.readthedocs.io/pt/latest/index.html"
                    //        }
                    //    });

                    IActivity activity = MessageFactory.Attachment(new HeroCard
                    {
                        Buttons = new List<CardAction>
                        {
                            new CardAction
                            {
                                Title = "Abrir manual",
                                Type = ActionTypes.OpenUrl,
                                Value = "https://pizzeria-bot-tc.readthedocs.io/pt/latest/index.html"
                            }
                        }
                    }.ToAttachment());

                    await dialogContext.Context.SendActivity($"Clique no botão abaixo para abrir o manual {Emojis.SmileHappy} ");
                    await dialogContext.Context.SendActivity(activity);
                }
                else
                {
                    await dialogContext.Continue();

                    BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);

                    if (!turnContext.Responded)
                    {
                        RecognizerResult luisResult = turnContext.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                        string intentResult = LuisResult.GetLuisIntent(luisResult, userState);

                        IDictionary<string, object> args = new Dictionary<string, object>
                        {
                            { "entities", EntitiesParse.RecognizeEntities(luisResult.Entities) }
                        };

                        await dialogContext.Begin(intentResult, args);
                    }
                }

            }
            else if (turnContext.Activity.Type != ActivityTypes.ConversationUpdate)
            {
                await turnContext.SendActivity($"Olá, ainda não estou preparado para tratar este tipo de informacão {Emojis.SmileSad}  \n" +
                    $"Peço que utilize apenas texto para melhorar nossa interação {Emojis.SmileHappy}");
            }
        }

    }

}
