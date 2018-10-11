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
using System.Threading;
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
        private readonly int quantityPizza;
        private readonly int quantityDrink;

        public AskProduct()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
            quantityPizza = 2;
            quantityDrink = 2;
        }

        #region Async Methods

        public async Task Ask_Product(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });

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
                pizzas = pizzas.Skip(quantityPizza * userState.EntitiesState.PizzasQuantityUsed).ToList();

                List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                userState.EntitiesState.PizzasQuantityUsed++;

                IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity($"Estou lhe enviando as pizzas disponíveis, começando pelas pizzas que possuem {GetIngredientsFindedText(pizzas, entities.Ingredients)}");
                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                Thread.Sleep(4000);
                await dialogContext.Context.SendActivity(messageActivity);
            }
            else if(entities.Drinks.Count > 0)
            {
                List<Drink> drinks = EntitieRecomendation.GetDrinksMoreSalesWithUserDrinks(userState.EntitiesState.EntitiesParse.Drinks, context);

                drinks = drinks.Skip(quantityDrink * userState.EntitiesState.DrinksQuantityUsed).ToList();

                List<Attachment> attachments = GetDrinkAttachments(drinks, userState);
                userState.EntitiesState.DrinksQuantityUsed++;

                IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity("Estou lhe enviando as bebidas disponíveis, começando pelas solicitadas");
                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                Thread.Sleep(4000);
                await dialogContext.Context.SendActivity(messageActivity);

            }
            else if(entities.ProductTypes.Count > 0)
            {
                if(entities.ProductTypes.Where(x => x == "pizza").ToList().Count > 0)
                {
                    List<Pizza> pizzas = EntitieRecomendation.GetPizzasByIngredients(userState.EntitiesState.EntitiesParse.Ingredients, context);
                    pizzas = pizzas.Skip(quantityPizza * userState.EntitiesState.PizzasQuantityUsed).ToList();

                    List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                    userState.EntitiesState.PizzasQuantityUsed++;

                    IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                    await dialogContext.Context.SendActivity("Estou lhe enviando as pizzas disponíveis, ordenando pelas mais vendidas");
                    await dialogContext.Context.SendActivity(new Activity
                    {
                        Type = ActivityTypes.Typing
                    });
                    Thread.Sleep(4000);
                    await dialogContext.Context.SendActivity(messageActivity);
                }
                else if (entities.ProductTypes.Where(x => x == "bebida").ToList().Count > 0)
                {
                    List<Drink> drinks = EntitieRecomendation.GetDrinksMoreSalesWithUserDrinks(userState.EntitiesState.EntitiesParse.Drinks, context);

                    drinks = drinks.Skip(quantityDrink * userState.EntitiesState.DrinksQuantityUsed).ToList();

                    List<Attachment> attachments = GetDrinkAttachments(drinks, userState);
                    userState.EntitiesState.DrinksQuantityUsed++;

                    IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                    await dialogContext.Context.SendActivity("Estou lhe enviando as bebidas disponíveis, ordenando pelas mais vendidas");
                    await dialogContext.Context.SendActivity(new Activity
                    {
                        Type = ActivityTypes.Typing
                    });
                    Thread.Sleep(4000);
                    await dialogContext.Context.SendActivity(messageActivity);
                }
            }
            else
            {
                List<Pizza> pizzas = EntitieRecomendation.GetPizzasMoreSales(context);
                pizzas = pizzas.Skip(quantityPizza * userState.EntitiesState.PizzasQuantityUsed).ToList();

                List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                userState.EntitiesState.PizzasQuantityUsed++;

                IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity("Estou lhe enviando as pizzas disponíveis, ordenando pelas mais vendidas");
                await dialogContext.Context.SendActivity(new Activity
                {
                    Type = ActivityTypes.Typing
                });
                Thread.Sleep(2000);
                await dialogContext.Context.SendActivity(messageActivity);
            }
        }

        public async Task OrderProducts(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });

            Activity activity = (Activity)args["Activity"];
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            List<Pizza> pizzas = EntitieRecomendation.GetPizzasByIngredients(userState.EntitiesState.EntitiesParse.Ingredients, context);
            if(userState.EntitiesState.EntitiesParse.Ingredients.Count == 0)
            {
                pizzas = EntitieRecomendation.GetPizzasMoreSales(context);
            }
            List<Drink> drinks = EntitieRecomendation.GetDrinksMoreSalesWithUserDrinks(userState.EntitiesState.EntitiesParse.Drinks, context);

            if (activity.Text.Contains(ActionTypes.PostBack + "AddPizza"))
            {
                string[] productData = activity.Text.Split("||");
                PizzaModel pizzaModel = AddPizzaOrder(productData[0], productData[1], userState);
                userState.EntitiesState.PizzasQuantityUsed--;

                await dialogContext.Context.SendActivity($"A pizza {pizzaModel.PizzaName} - {pizzaModel.SizeName} foi adicionada com sucesso {Emojis.SmileHappy}");
                await dialogContext.Context.SendActivity($"Gostaria de ver mais alguma pizza?  \nClique no botão caso deseje, ou simplesmente solicite o que deseja {Emojis.SmileHappy})");

                await dialogContext.Context.SendActivity(GetSuggestedActionsNewsPizzasAndDrinks("Pizza"));
            }
            else if (activity.Text.Contains(ActionTypes.PostBack + "SuggestedActionPizza"))
            {
                pizzas = pizzas.Skip(quantityPizza * userState.EntitiesState.PizzasQuantityUsed).ToList();
                List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                userState.EntitiesState.PizzasQuantityUsed++;
                IMessageActivity activitySend = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity(activitySend);
            }
            else if (activity.Text == (ActionTypes.PostBack + "MorePizza"))
            {
                pizzas = pizzas.Skip(quantityPizza * userState.EntitiesState.PizzasQuantityUsed).ToList();
                List<Attachment> attachments = GetPizzaAttachments(pizzas, userState);
                userState.EntitiesState.PizzasQuantityUsed++;
                IMessageActivity activitySend = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity(activitySend);
            }
            else if (activity.Text.Contains(ActionTypes.PostBack + "AddDrink"))
            {
                string[] productData = activity.Text.Split("||");
                DrinkModel drinkModel = AddDrinkOrder(productData[0], productData[1], userState);
                userState.EntitiesState.DrinksQuantityUsed--;

                await dialogContext.Context.SendActivity($"{drinkModel.DrinkName} {drinkModel.DrinkSizeName} adicionado com sucesso {Emojis.SmileHappy}");
                await dialogContext.Context.SendActivity($"Gostaria de ver mais alguma bebida?  \nClique no botão caso deseje, ou simplesmente solicite o que deseja {Emojis.SmileHappy}");

                await dialogContext.Context.SendActivity(GetSuggestedActionsNewsPizzasAndDrinks("Drink"));
            }
            else if (activity.Text.Contains(ActionTypes.PostBack + "SuggestedActionDrink"))
            {
                drinks = drinks.Skip(quantityDrink * userState.EntitiesState.DrinksQuantityUsed).ToList();
                List<Attachment> attachments = GetDrinkAttachments(drinks, userState);
                userState.EntitiesState.DrinksQuantityUsed++;
                IMessageActivity activitySend = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity(activitySend);
            }
            else if (activity.Text == (ActionTypes.PostBack + "MoreDrink"))
            {
                drinks = drinks.Skip(quantityDrink * userState.EntitiesState.DrinksQuantityUsed).ToList();
                List<Attachment> attachments = GetDrinkAttachments(drinks, userState);
                userState.EntitiesState.DrinksQuantityUsed++;
                IMessageActivity messageActivity = MessageFactory.Carousel(attachments);
                await dialogContext.Context.SendActivity(messageActivity);
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

        //Just return to the conversation
        public async Task AnswerAsk_Product(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Replace(Order_Product_Waterfall_Text, args);
        }

        public async Task AnswerOrderProducts(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Replace(Order_Product_Waterfall_Text, args);
        }

        #endregion

        #region Private Methods

        private IActivity GetSuggestedActionsNewsPizzasAndDrinks(string type)
        {
            string text = type == "Pizza" ? "Mais pizzas" : "Mais bebidas";

            return MessageFactory.Attachment(new HeroCard
            {
                Buttons = new List<CardAction>
                {
                    new CardAction
                     {
                         Title = text,
                         Type = ActionTypes.PostBack,
                         Value = "quero||" + ActionTypes.PostBack + "SuggestedAction" + type,
                     }
                }
            }.ToAttachment());

            //return MessageFactory.SuggestedActions(
            //    new CardAction[]
            //    {
            //        new CardAction
            //        {
            //            Title = "Quero",
            //            Type = ActionTypes.PostBack,
            //            Value = "quero||" + ActionTypes.PostBack + "SuggestedAction" + type,
            //        }
            //    });
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

            foreach (var pizza in pizzas.Take(quantityPizza))
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

            if (pizzas.Count > quantityPizza * userState.EntitiesState.PizzasQuantityUsed)
            {
                attachments.Add(new HeroCard
                {
                    Buttons = new List<CardAction>
                    {
                        new CardAction { Title = "Mais Pizzas", Value = ActionTypes.PostBack + "MorePizza", Type = ActionTypes.PostBack }
                    }
                }.ToAttachment());
            }
            return attachments;
        }

        private List<Attachment> GetDrinkAttachments(List<Drink> drinks, BotUserState userState)
        {
            List<Attachment> attachments = new List<Attachment>();

            foreach (var drink in drinks.Take(quantityDrink))
            {
                attachments.Add(new ThumbnailCard
                {
                    Title = drink.Name,
                    Images = new CardImage[] { new CardImage(url: ServerUrl + @"/" + drink.Image) },
                    Buttons = drink.DrinkSizes.Select(x => new CardAction
                    {
                        Type = ActionTypes.PostBack,
                        Title = x.SizeD.SizeName + " ( R$ " + x.Price.ToString("F") + ")",
                        Value = drink.DrinkId + "||" + x.SizeD.Quantity + "||" + ActionTypes.PostBack + "AddDrink"
                    }).ToList()
                }.ToAttachment());
            }

            if (drinks.Count > quantityDrink * userState.EntitiesState.DrinksQuantityUsed)
            {
                attachments.Add(new HeroCard
                {
                    Buttons = new List<CardAction>
                    {
                        new CardAction { Title = "Mais bebidas", Value = ActionTypes.PostBack + "MoreDrink", Type = ActionTypes.PostBack }
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
            PizzaModel pizzaModelFind = userState.OrderModel.Pizzas.Where(x => x.PizzaId == int.Parse(id) && x.SizeName == size).FirstOrDefault();

            if (pizzaModelFind != null)
            {
                pizzaModelFind.Quantity++;
                userState.OrderModel.PriceTotal += pizzaModelFind.Price;
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
                    SizeId = pizzaSize.SizePId,
                    SizeName = pizzaSize.SizeP.Size,
                    Quantity = 1,
                    Price = pizzaSize.Price
                };
                userState.OrderModel.Pizzas.Add(pizzaModel);
                userState.OrderModel.PriceTotal += (pizzaModel.Quantity * pizzaModel.Price);
                return pizzaModel;
            }
        }

        private DrinkModel AddDrinkOrder(string id, string quantity, BotUserState userState)
        {
            DrinkModel drinkModelFind = userState.OrderModel.Drinks.Where(x => x.DrinkId == int.Parse(id) && x.DrinkQuantity == double.Parse(quantity)).FirstOrDefault();

            if (drinkModelFind != null)
            {
                drinkModelFind.Quantity++;
                userState.OrderModel.PriceTotal += drinkModelFind.Price;
                return drinkModelFind;
            }
            else
            {
                Drink drink = context.Drinks.Where(x => x.DrinkId == int.Parse(id)).Include(x => x.DrinkSizes).ThenInclude(y => y.SizeD).FirstOrDefault();
                DrinkSize drinkSize = drink.DrinkSizes
                    .Where(x => x.SizeD.Quantity == double.Parse(quantity)).FirstOrDefault();


                DrinkModel drinkModel = new DrinkModel
                {
                    DrinkId = drink.DrinkId,
                    DrinkName = drink.Name,
                    DrinkQuantity = drinkSize.SizeD.Quantity,
                    Quantity = 1,
                    DrinkSizeName = drinkSize.SizeD.SizeName,
                    Price = drinkSize.Price
                };

                userState.OrderModel.Drinks.Add(drinkModel);
                userState.OrderModel.PriceTotal += (drinkModel.Quantity * drinkModel.Price);
                return drinkModel;
            }
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
