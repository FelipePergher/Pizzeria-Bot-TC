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
        None None;

        public DialogFlow()
        {
            SalutationTypes = new SalutationTypes();
            None = new None();

            Add(SalutationTypes.TextPrompt, new TextPrompt());

            Add(None.NoneText, None.NoneWaterfall());

            Add(SalutationTypes.SalutationText, SalutationTypes.SalutationWaterfall());

            Add(SalutationTypes.Salutation_How_Is_Text, SalutationTypes.Salutation_How_Is_Waterfall());

            Add(SalutationTypes.How_Is_Text, SalutationTypes.How_Is_Waterfall());

        }
    }
}
