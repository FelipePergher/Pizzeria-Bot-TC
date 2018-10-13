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
                await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pergher Pizzaria!!!");

                await dialogContext.Context.SendActivity($"Eu sou o Jorge {Emojis.ManRaisingHand} o bot da Pergher pizzaria. Estou aqui para te ajudar em seu pedido.  \n" +
                    $"Dúvidas? Digite *AJUDA* e será encaminhado ao manual de utilização.  \n" +
                    $"Problemas em alguma parte da conversa? Digite *SAIR* e voltaremos ao fluxo normal.");

                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                await dialogContext.Prompt(TextPrompt, "Como você está se sentindo hoje?");
            }
            else
            {
                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                await dialogContext.Context.SendActivity($"Olá, é um prazer ter você aqui {Emojis.SmileHappy}  \nO que você gostaria hoje? ");
                await dialogContext.End();
            }
            
        }
        private async Task Salutation_How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            await dialogContext.Context.SendActivity("Olá, seja bem vindo a Pergher Pizzaria!!!");
            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });
            await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
        }
        private async Task How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });
            await dialogContext.Prompt(TextPrompt, "Eu estou ótimo e você, como está se sentindo hoje?");
        }
        private async Task Answer(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State["status"] = args["Value"];

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Status = Convert.ToString(dialogContext.ActiveDialog.State["status"]);

            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });

            await dialogContext.Context.SendActivity($"Legal!  \n O que você gostaria hoje?");
            await dialogContext.End();
        }
        private async Task Answer_How_Is(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State["status"] = args["Value"];

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Status = Convert.ToString(dialogContext.ActiveDialog.State["status"]);

            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });

            await dialogContext.Context.SendActivity($"Eu estou ótimo {Emojis.SmileHappy}!  \nO que você gostaria hoje?");
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
