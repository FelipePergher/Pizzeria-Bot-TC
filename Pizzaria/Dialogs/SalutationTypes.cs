using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
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
        public const string SalutationWaterfallText = "Salutation";
        public const string Salutation_How_Is_Waterfall_Text = "Salutation_How_Is";
        public const string How_Is_Waterfall_Text = "How_Is";

        #region Async Methods

        private async Task Salutation(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (string.IsNullOrEmpty(userState.Status))
            {
                dialogContext.ActiveDialog.State = new Dictionary<string, object>();
                await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pizzaria do Manolo!!!");

                await dialogContext.Context.SendActivity($"Eeu sou o Jorge {Emojis.SmileHappy} o _bot_ da pizzaria do Manolo que estou aqui para auxiliá-lo em seu pedido\n" +
                    $"Caso possua alguma duvida simplesmente digite *AJUDA* e lhe encaminharei para exemplos de utilização {Emojis.SmileHappy}\n" +
                    $"Caso queira sair de alguma parte da conversa simplesmente digite *SAIR* e voltaremos ao fluxo normal da conversa {Emojis.SmileHappy}\n" +
                    $"Em que lhe posso ser útil no momento?");

                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                await dialogContext.Prompt(TextPrompt, "Como você está se sentindo hoje?");
            }
            else
            {
                await dialogContext.Context.SendActivity($"Olá {Emojis.SmileHappy}, é um prazer tê-lo aqui {Emojis.SmileHappy} ");
                await dialogContext.End();
            }
            
        }
        private async Task Salutation_How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (string.IsNullOrEmpty(userState.Status))
            {
                await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pizzaria do Manolo!!!");
                await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
            }
            else
            {
                await dialogContext.Context.SendActivity($"Olá {Emojis.SmileHappy}, é um prazer tê-lo aqui {Emojis.SmileHappy} ");
                await dialogContext.End();
            }
        }
        private async Task How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (string.IsNullOrEmpty(userState.Status))
            {
                await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
            }
            else
            {
                await dialogContext.Context.SendActivity($"Olá {Emojis.SmileHappy}, é um prazer tê-lo aqui {Emojis.SmileHappy} ");
                await dialogContext.End();
            }
        }
        private async Task Answer(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State["status"] = args["Value"];

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Status = Convert.ToString(dialogContext.ActiveDialog.State["status"]);

            await dialogContext.Context.SendActivity($"Legal!");
            await dialogContext.Context.SendActivity($"O que você gostaria hoje?");
            await dialogContext.End();
        }
        private async Task Answer_How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State["status"] = args["Value"];

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Status = Convert.ToString(dialogContext.ActiveDialog.State["status"]);

            await dialogContext.Context.SendActivity($"Eu estou ótimo!");
            await dialogContext.Context.SendActivity($"O que você gostaria hoje?");
            await dialogContext.End();
        }

        #endregion

        #region Waterfall

        public WaterfallStep[] SalutationWaterfall()
        {
            return new WaterfallStep[]
            {
                Salutation,
                Answer_How_Is
            };
        }

        public WaterfallStep[] Salutation_How_Is_Waterfall()
        {
            return new WaterfallStep[]
            {
                Salutation_How_Is,
                Answer
            };
        }

        public WaterfallStep[] How_Is_Waterfall()
        {
            return new WaterfallStep[]
            {
                How_Is,
                Answer
            };
        }

        #endregion
    }
}
