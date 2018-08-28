using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.Ai.LUIS;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pizzaria.Dialogs
{
    public class Bot : IBot
    {
        private const double LUIS_INTENT_THRESHOLD = 0.7d;
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

                if (!turnContext.Responded)
                {
                    var luisResult = turnContext.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                    var (intent, score) = luisResult.GetTopScoringIntent();
                    var intentResult = score > LUIS_INTENT_THRESHOLD ? intent : "None";
                    await dialogContext.Begin(intentResult);
                }

            }
        }

        private Task None(DialogContext dialogContext, object args, SkipStepFunction next)
        {
            return dialogContext.Context.SendActivity("None");
        }

    }

}
