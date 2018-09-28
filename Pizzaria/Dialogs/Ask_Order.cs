using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class Ask_Order : DialogSet
    {
        public string TextPrompt { get; set; } = "textPrompt";
        public const string Ask_Order_WaterfallText = "Ask_Order";
        public const string Clean_Order_WaterfallText = "Clean_Order";

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

        private async Task Clean_OrderBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity("Você quer realmente limpar seu carrinho?");
            await dialogContext.Context.SendActivity(MessageFactory.SuggestedActions(
                new CardAction[]
                {
                    new CardAction
                    {
                        Title = "Sim",
                        Type = ActionTypes.PostBack,
                        Value = ActionTypes.PostBack + "CleanOrder||true"
                    },
                    new CardAction
                    {
                        Title = "Nâo",
                        Type = ActionTypes.PostBack,
                        Value = "CleanOrder||false"
                    }
                })
            );
        }

        private async Task AnswerClean_OrderBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            Activity activity = (Activity)args["Activity"];
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (activity.Text.Contains(ActionTypes.PostBack + "CleanOrder"))
            {
                bool answer = activity.Text.Split("||")[1] == "true" ? true : false;
                if (answer)
                {
                    userState.Order.Drinks.Clear();
                    userState.Order.Pizzas.Clear();
                    userState.Order.PriceTotal = 0;
                    userState.Order.QuantityTotal = 0;
                    await dialogContext.Context.SendActivity("Seu carrinho foi limpo, o que você gostaria agora?");
                }
                else
                {
                    await dialogContext.Context.SendActivity("Sinto muito mas algo deu errado :(");
                }
            }
            else
            {
                RecognizerResult luisResult = dialogContext.Context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);
                string intentResult = LuisResult.GetLuisIntent(luisResult, userState);

                IDictionary<string, object> createdArgs = new Dictionary<string, object>
                {
                    { "entities", EntitiesParse.RecognizeEntities(luisResult.Entities) }
                };
                await dialogContext.Replace(intentResult, createdArgs);
            }
        }

        #endregion

        #region Private Methods

        private ReceiptCard GetReceiptCart(BotUserState userState)
        {
            List<ReceiptItem> receiptItems = new List<ReceiptItem>();
            foreach (var item in userState.Order.Pizzas)
            {
                Pizza pizza = context.Pizzas.Where(x => x.PizzaId == item.PizzaId).Include(x => x.PizzaIngredients).ThenInclude(y => y.Ingredient).FirstOrDefault();
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

            foreach (var item in userState.Order.Drinks)
            {
                Drink drink = context.Drinks.Find(item.DrinkId);
                double price = item.Price * item.Quantity;
                receiptItems.Add(new ReceiptItem
                {
                    Title = $"({item.Quantity}) {item.DrinkName}",
                    Subtitle = item.DrinkSizeName,
                    Price = "R$" + price.ToString("F"),
                    Quantity = item.Quantity.ToString(),
                    Image = new CardImage(url: ServerUrl + @"/" + drink.Image)
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

        public WaterfallStep[] Clean_OrderWaterfall()
        {
            return new WaterfallStep[]
            {
                Clean_OrderBegin,
                AnswerClean_OrderBegin
            };
        }

        #endregion
    }
}
