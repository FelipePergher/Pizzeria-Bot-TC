﻿using Microsoft.Bot.Builder.Dialogs;
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
        Ask_Status Ask_Status;
        AskOrder Ask_Order;
        Thanks Thanks;
        None None;
        WhoAre WhoAre;

        public DialogFlow()
        {
            SalutationTypes = new SalutationTypes();
            AskProduct = new AskProduct();
            Ask_Order = new AskOrder();
            None = new None();
            Thanks = new Thanks();            
            Ask_Status = new Ask_Status();
            WhoAre = new WhoAre();

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

            Add(Ask_Status.AskStatusText, Ask_Status.Ask_StatusWaterfall());

            Add(Thanks.ThanksWaterfallText, Thanks.ThanksWaterfall());

            Add(AskOrder.EditAddressWaterfallText, Ask_Order.EditAddressWaterfall());

            Add(AskOrder.EditOrderWaterfallText, Ask_Order.EditOrderWaterfall());

            Add(WhoAre.WhoAreWaterfallText, WhoAre.WhoAreWaterfall());
                
        }
    }
}
