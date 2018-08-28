using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class DialogFlow : DialogSet
    {
        SalutationTypes SalutationTypes;

        public DialogFlow()
        {
            SalutationTypes = new SalutationTypes();

            Add(SalutationTypes.TextPrompt, new TextPrompt());

            Add(SalutationTypes.SalutationBegin, new WaterfallStep[]
            {
                SalutationTypes.Salutation,
                SalutationTypes.Answer
            });

            Add(SalutationTypes.Salutation_How_Is_Begin, new WaterfallStep[]
            {
                SalutationTypes.Salutation_How_Is,
                SalutationTypes.Answer
            });

            Add(SalutationTypes.How_Is_Begin, new WaterfallStep[]
            {
                SalutationTypes.How_Is,
                SalutationTypes.Answer
            });
        }
    }
}
