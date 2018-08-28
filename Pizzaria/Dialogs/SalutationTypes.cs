using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class SalutationTypes : DialogSet
    {
        public string TextPrompt { get; set; } = "textPrompt";
        public const string SalutationBegin = "Salutation";
        public const string Salutation_How_Is_Begin = "Salutation_How_Is";
        public const string How_Is_Begin = "How_Is";

        public async Task Salutation(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State = new Dictionary<string, object>();
            await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pizzaria do Manolo!!!");
            await dialogContext.Prompt(TextPrompt, "Como você está se sentindo hoje?");
        }
        public async Task Salutation_How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pizzaria do Manolo!!!");
            await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
        }
        public async Task How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
        }
        public async Task Answer(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State["status"] = args["Value"];

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Status = Convert.ToString(dialogContext.ActiveDialog.State["status"]);

            await dialogContext.Context.SendActivity($"Que bom que você está: {userState.Status}!");
            await dialogContext.End();
        }
    }
}
