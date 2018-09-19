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
            if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate && turnContext.Activity.MembersAdded.FirstOrDefault()?.Id == turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivity("Bem vindo a conversa.");
            }
            else if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogState = turnContext.GetConversationState<Dictionary<string, object>>();

                DialogContext dialogContext = DialogFlow.CreateContext(turnContext, dialogState);

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

                    //Todo: Remover depois. Usado para ir direto para o pedido
                    //await dialogContext.Begin(intentResult, args);
                    await dialogContext.Begin(AskProduct.Ask_Product_Waterfall_Text, args);
                }
            }
            else if(turnContext.Activity.Type != ActivityTypes.ConversationUpdate)
            {
                await turnContext.SendActivity("Evento não tratado -> " + turnContext.Activity.Type);
            }
        }
        
    }

}
