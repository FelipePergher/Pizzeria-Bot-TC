using Microsoft.Bot.Builder.Dialogs;
using Pizzaria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class DialogFlow : DialogSet
    {
        SalutationTypes SalutationTypes;
        AskProduct AskProduct;
        AskOrder Ask_Order;
        None None;

        public DialogFlow()
        {
            SalutationTypes = new SalutationTypes();
            AskProduct = new AskProduct();
            Ask_Order = new AskOrder();
            None = new None();

            Add(SalutationTypes.TextPrompt, new TextPrompt());

            Add(None.NoneText, None.NoneWaterfall());

            Add(SalutationTypes.SalutationWaterfallText, SalutationTypes.SalutationWaterfall());

            Add(SalutationTypes.Salutation_How_Is_Waterfall_Text, SalutationTypes.Salutation_How_Is_Waterfall());

            Add(SalutationTypes.How_Is_Waterfall_Text, SalutationTypes.How_Is_Waterfall());

            Add(AskProduct.Ask_Product_Waterfall_Text, AskProduct.Ask_ProductWaterfall());

            Add(AskProduct.Order_Product_Waterfall_Text, AskProduct.OrderProductsWaterfall());

            Add(AskOrder.Ask_Order_WaterfallText, Ask_Order.Ask_OrderWaterfall());

            Add(AskOrder.Clean_Order_WaterfallText, Ask_Order.Clean_OrderWaterfall());

            Add(AskOrder.End_Order_WaterfallText, Ask_Order.End_OrderWaterfall());

            Add(AskOrder.AskUserAddressWaterfallText, Ask_Order.AskUserAddressWaterfall());

            Add(AskOrder.ReuseUserAddressWaterfallText, Ask_Order.ReuseUserAddressWaterfall());
                
        }
    }
}
