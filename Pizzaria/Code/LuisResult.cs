﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Pizzaria.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public class LuisResult
    {
        private const double LUIS_INTENT_THRESHOLD = 0.75d;

        public static string GetLuisIntent(RecognizerResult luisResult, BotUserState userState)
        {
            var (intent, score) = luisResult.GetTopScoringIntent();
            var intentResult = score > LUIS_INTENT_THRESHOLD ? intent : "None";

            if (!string.IsNullOrEmpty(userState.Status) && (intentResult == SalutationTypes.How_Is_Waterfall_Text ||
                intentResult == SalutationTypes.SalutationWaterfallText ||
                intentResult == SalutationTypes.Salutation_How_Is_Waterfall_Text))
            {
                intentResult = "None";
            }
            return intentResult;
        }
    }
}
