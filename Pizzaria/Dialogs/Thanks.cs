using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class Thanks : DialogSet
    {
        public const string ThanksWaterfallText = "Thanks";

        private async Task Thank(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            dialogContext.ActiveDialog.State = new Dictionary<string, object>();
            await dialogContext.Context.SendActivity($"De nada, eu que agradeço a preferência {Emojis.SmileHappy}");
        }


        public WaterfallStep[] ThanksWaterfall()
        {
            return new WaterfallStep[]
            {
                Thank
            };
        }
    }
}
