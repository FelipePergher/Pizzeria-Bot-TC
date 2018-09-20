using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class AskProduct
    {
        public const string Ask_Product_Waterfall_Text = "Ask_Product";
        public const string Order_Product_Waterfall_Text = "Order_Product_Text";
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;
        private readonly string ServerUrl;
        private readonly int quantityProduct;

        public AskProduct()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
            quantityProduct = 1;
        }

        #region Async Methods

        public async Task Ask_Product(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            EntitiesParse entities = (EntitiesParse)args["entities"];
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.EntitiesState = new EntitiesState
            {
                EntitiesParse = entities,
                AddedDate = DateTime.Now,
                DrinksQuantityUsed = 0,
                PizzasQuantityUsed = 0
            };

            if (entities.Ingredients.Count > 0)
            {
                List<Pizza> pizzas = EntitieRecomendation.GetPizzasByIngredients(entities.Ingredients, context);
                pizzas = pizzas.Skip(quantityProduct * userState.EntitiesState.PizzasQuantityUsed).ToList();
                userState.EntitiesState.PizzasQuantityUsed++;

                List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity($"Encontrei as seguintes pizzas que possuem {GetIngredientsFindedText(pizzas, entities.Ingredients)}");
                await dialogContext.Context.SendActivity(messageActivity);
            }
            else
            {
                //Todo: oferecerer as pizzas mais vendidas se não tiver bebidas nas entidades
                await dialogContext.Context.SendActivity("nenhum ingrediente nas entidades");
            }
        }

        public async Task AnswerAsk_Product(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Replace(Order_Product_Waterfall_Text, args);
        }

        public async Task OrderProducts(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            Activity activity = (Activity)args["Activity"];
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            List<Pizza> pizzas = EntitieRecomendation.GetPizzasByIngredients(userState.EntitiesState.EntitiesParse.Ingredients, context);

            if (activity.Text.Contains(ActionTypes.PostBack + "AddPizza"))
            {
                string[] productData = activity.Text.Split("||");
                PizzaModel pizzaModel = AddPizzaOrder(productData[0], productData[1], userState);

                //Todo: oferecer se o usuário quer ver mais pizzas
                await dialogContext.Context.SendActivity($"A pizza {pizzaModel.PizzaName} foi adicionada com sucesso :)");
                await dialogContext.Context.SendActivity("Gostaria de ver mais alguma pizza? (Clique em quero caso deseje, ou simplesmente solicite o que deseja :))");

                await dialogContext.Context.SendActivity(GetSuggestedActionsNewsPizzas());
            }
            else if (activity.Text.Contains(ActionTypes.PostBack + "SuggestedAction"))
            {
                await dialogContext.Context.SendActivity("resposta do suggested action");
            }
            else if (activity.Text == (ActionTypes.PostBack + "More"))
            {
                pizzas = pizzas.Skip(quantityProduct * userState.EntitiesState.PizzasQuantityUsed).ToList();
                if (pizzas.Count > 0)
                {
                    List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                    userState.EntitiesState.PizzasQuantityUsed++;
                    IMessageActivity activitySend = MessageFactory.Carousel(attachments);
                    await dialogContext.Context.SendActivity(activitySend);
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
                //Todo: voltar a usar a entidade normalmente depois de ajustar
                await dialogContext.Begin(AskProduct.Ask_Product_Waterfall_Text, createdArgs);
                //await dialogContext.Replace(intentResult, createdArgs);
            }
        }

        public async Task AnswerOrderProducts(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Replace(Order_Product_Waterfall_Text, args);
        }

        #endregion

        #region Private Methods

        private IActivity GetSuggestedActionsNewsPizzas()
        {
            return MessageFactory.SuggestedActions(
                new CardAction[]
                {
                    new CardAction
                    {
                        Title = "Quero",
                        Type = ActionTypes.PostBack,
                        Value = "quero||" + ActionTypes.PostBack + "SuggestedAction",
                        Text = "asdasdasdsas"
                        
                    }
                });
        }

        private string GetIngredientsFindedText(List<Pizza> pizzas, List<string> ingredients)
        {
            List<string> allIngredients = new List<string>();
            foreach (var pizza in pizzas)
            {
                allIngredients.AddRange(pizza.PizzaIngredients.Select(y => y.Ingredient.Name).ToList());
            }

            allIngredients = allIngredients.Distinct().ToList();
            List<string> ingredientsFinded = new List<string>();

            foreach (var ingredient in ingredients)
            {
                if (allIngredients.Contains(ingredient))
                {
                    ingredientsFinded.Add(ingredient);
                }
            }

            string returnString = "";

            foreach (var ingredientsFind in ingredientsFinded.Select((value, i) => new { i, value }))
            {
                if (ingredientsFind.i == 0)
                {
                    returnString += ingredientsFind.value;
                }
                else if (ingredientsFind.i == ingredientsFinded.Count - 1)
                {
                    returnString += " e " + ingredientsFind.value;
                }
                else
                {
                    returnString += ", " + ingredientsFind.value;
                }
            }

            return returnString;
        }

        private List<Attachment> GetPizzaAttachments(List<Pizza> pizzas, BotUserState userState)
        {
            List<Attachment> attachments = new List<Attachment>();

            foreach (var pizza in pizzas.Take(quantityProduct))
            {
                string ingredients = GetIngredientsString(pizza.PizzaIngredients);

                attachments.Add(new ThumbnailCard
                {
                    Title = pizza.Name,
                    Subtitle = pizza.PizzaType,
                    Text = ingredients,
                    Images = new CardImage[] { new CardImage(url: ServerUrl + @"/" + pizza.Image) },
                    Buttons = pizza.PizzaSizes.Select(x => new CardAction
                    {
                        Type = ActionTypes.PostBack,
                        Title = x.SizeP.Size + " ( R$ " + x.Price.ToString("F") + ")",
                        Value = pizza.PizzaId + "||" + x.SizeP.Size + "||" + ActionTypes.PostBack + "AddPizza"
                    }).ToList()
                }.ToAttachment());
            }

            if (pizzas.Count > quantityProduct * userState.EntitiesState.PizzasQuantityUsed)
            {
                attachments.Add(new HeroCard
                {
                    Images = new List<CardImage>
                        {
                            new CardImage
                            {
                                Url = ServerUrl + @"/Images/Icons/more-button.png",
                                Tap = new CardAction { Title = "Mais Pizzas", Value = ActionTypes.PostBack + "More", Type = ActionTypes.PostBack }
                            }
                        }
                }.ToAttachment());
            }
            return attachments;
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

        private PizzaModel AddPizzaOrder(string id, string size, BotUserState userState)
        {
            PizzaModel pizzaModelFind = userState.Order.Pizzas.Where(x => x.PizzaId == int.Parse(id) && x.SizeName == size).FirstOrDefault();

            if (pizzaModelFind != null)
            {
                pizzaModelFind.Quantity++;
                userState.Order.PriceTotal += pizzaModelFind.Price;
                return pizzaModelFind;
            }
            else
            {
                Pizza pizza = context.Pizzas.Where(x => x.PizzaId == int.Parse(id)).FirstOrDefault();
                PizzaSize pizzaSize = context.Pizzas.Include(x => x.PizzaIngredients).ThenInclude(y => y.Ingredient)
                    .Include(x => x.PizzaSizes).ThenInclude(y => y.SizeP).FirstOrDefault().PizzaSizes.Where(x => x.SizeP.Size == size).FirstOrDefault();

                PizzaModel pizzaModel = new PizzaModel
                {
                    PizzaId = pizza.PizzaId,
                    PizzaName = pizza.Name,
                    PizzaSizeId = pizzaSize.PizzaSizeId,
                    SizeId = pizzaSize.SizePId,
                    SizeName = pizzaSize.SizeP.Size,
                    Quantity = 1,
                    Price = pizzaSize.Price
                };
                userState.Order.Pizzas.Add(pizzaModel);
                userState.Order.PriceTotal += (pizzaModel.Quantity * pizzaModel.Price);
                return pizzaModel;
            }
        }

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

        #endregion

        #region Waterfalls

        public WaterfallStep[] Ask_ProductWaterfall()
        {
            return new WaterfallStep[]
            {
                Ask_Product,
                AnswerAsk_Product
            };
        }

        public WaterfallStep[] OrderProductsWaterfall()
        {
            return new WaterfallStep[]
            {
                OrderProducts,
                AnswerOrderProducts
            };
        }

        #endregion
    }
}
