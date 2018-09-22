using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class Ask_Order : DialogSet
    {
        public const string Ask_Order_WaterfallText = "Ask_Order";

        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;
        private readonly string ServerUrl;

        public Ask_Order()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
        }

        #region Async Methods

        private async Task Ask_OrderBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);

            ReceiptCard receiptCard = GetReceiptCart(userState);
            if(receiptCard.Items.Count > 0)
            {
                IMessageActivity messageActivity = MessageFactory.Attachment(receiptCard.ToAttachment());
                await dialogContext.Context.SendActivity(messageActivity);
                //Todo: Oferecer edição do carinho
                await dialogContext.End();
            }
            else
            {
                await dialogContext.Context.SendActivity($"Você ainda não possui nada em seu carrinho :(");
                //Todo: enviar opções de compra
                await dialogContext.End();
            }
        }

        private async Task Answer_Ask_Orderbegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity("resposta do carrinho de compras");
        }

        #endregion

        #region Private Methods

        private ReceiptCard GetReceiptCart(BotUserState userState)
        {
            List<ReceiptItem> receiptItems = new List<ReceiptItem>();
            foreach (var item in userState.Order.Pizzas)
            {
                Pizza pizza = context.Pizzas.Find(userState.Order.Pizzas.First().PizzaId);
                double price = item.Price * item.Quantity;
                receiptItems.Add(new ReceiptItem
                {
                    Title = $"({item.Quantity}) {item.PizzaName} | {item.SizeName}",
                    Price = "R$" + price.ToString("F"),
                    Quantity = item.Quantity.ToString(),
                    Subtitle = GetIngredientsString(pizza.PizzaIngredients),
                    Image = new CardImage(url: ServerUrl + @"/" + pizza.Image)
                });
            }

            ReceiptCard receiptCard = new ReceiptCard
            {
                Title = "Dados da ordem",
                Total = "R$ " + userState.Order.PriceTotal.ToString("F"),
                Items = receiptItems
            };

            return receiptCard;
        }

        private string GetIngredientsString(ICollection<PizzaIngredient> pizzaIngredients)
        {
            string ingredients = "Ingredientes: ";
            foreach (var ingredient in pizzaIngredients.Select((value, i) => new { i, value }))
            {
                if (pizzaIngredients.Count - 1 == ingredient.i)
                {
                    ingredients += " e " + ingredient.value.Ingredient.Name;
                }
                else if (ingredient.i == 0)
                {
                    ingredients += ingredient.value.Ingredient.Name;
                }
                else
                {
                    ingredients += ", " + ingredient.value.Ingredient.Name;
                }
            }

            return ingredients;
        }


        #endregion


        #region Waterfall

        public WaterfallStep[] Ask_OrderWaterfall()
        {
            return new WaterfallStep[]
            {
                Ask_OrderBegin,
                Answer_Ask_Orderbegin
            };
        }

        #endregion
    }
}
