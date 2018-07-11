using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.Ai.LUIS;
using System.Collections.Generic;
using Prompts = Microsoft.Bot.Builder.Prompts;
using Microsoft.Recognizers.Text;

namespace Pizzaria.Dialogs
{
    public class Bot : IBot
    {

        private const double LUIS_INTENT_THRESHOLD = 0.7d;

        private readonly DialogSet dialogs;

        public Bot()
        {
            dialogs = new DialogSet();
            dialogs.Add("None", new WaterfallStep[] { None });
            dialogs.Add("Salutation", new WaterfallStep[] { Salutation });
            dialogs.Add("Salutation_How_Is", new WaterfallStep[] { Salutation_How_Is });
            dialogs.Add("How_Is", new WaterfallStep[] { How_Is });
        }

        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate && turnContext.Activity.MembersAdded.FirstOrDefault()?.Id == turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivity("Bem vindo a conversa.");
            }
            else if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var state = turnContext.GetConversationState<Dictionary<string, object>>();
                var dialogContext = dialogs.CreateContext(turnContext, state);

                if (!turnContext.Responded)
                {
                    var luisResult = turnContext.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                    var (intent, score) = luisResult.GetTopScoringIntent();
                    var intentResult = score > LUIS_INTENT_THRESHOLD ? intent : "None";

                    await dialogContext.Begin(intent);
                }
            }
        }

        private Task None(DialogContext dialogContext, object args, SkipStepFunction next)
        {
            return dialogContext.Context.SendActivity("None");
        }
        private Task Salutation(DialogContext dialogContext, object args, SkipStepFunction next)
        {
            return dialogContext.Context.SendActivity("Salutation");
        }
        private Task Salutation_How_Is(DialogContext dialogContext, object args, SkipStepFunction next)
        {
            return dialogContext.Context.SendActivity("Salutation How is");
        }
        private Task How_Is(DialogContext dialogContext, object args, SkipStepFunction next)
        {
            return dialogContext.Context.SendActivity("How is");
        }
    }
}
