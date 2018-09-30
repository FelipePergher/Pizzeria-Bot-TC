using AdaptiveCards;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.OrderModels;
using Pizzaria.Data.Models.PizzaModels;
using Pizzaria.Data.Models.UserModels;
using Pizzaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class AskOrder : DialogSet
    {
        public string TextPrompt { get; set; } = "textPrompt";
        public const string Ask_Order_WaterfallText = "Ask_Order";
        public const string Clean_Order_WaterfallText = "Clean_Order";
        public const string End_Order_WaterfallText = "End_Order";
        public const string AskUserAddressWaterfallText = "AskAddressUser";
        public const string ReuseUserAddressWaterfallText = "ReuseAddressUser";

        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;
        private readonly string ServerUrl;

        public AskOrder()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
        }

        #region Ask Order Dialog

        private async Task Ask_OrderBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);

            ReceiptCard receiptCard = GetReceiptCart(userState);
            if (receiptCard.Items.Count > 0)
            {
                IMessageActivity messageActivity = MessageFactory.Attachment(receiptCard.ToAttachment());
                await dialogContext.Context.SendActivity(messageActivity);
                //Todo: Oferecer edição do carinho
                await dialogContext.End();
            }
            else
            {
                await dialogContext.Context.SendActivity($"Você ainda não possui nada em seu carrinho :( \n Mas estou enviando algumas pizzas para você ver :) ");

                EntitiesParse entities = (EntitiesParse)args["entities"];

                await dialogContext.Replace(AskProduct.Ask_Product_Waterfall_Text, args);

            }
        }

        private async Task Answer_Ask_Orderbegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity("resposta do carrinho de compras");
        }

        #endregion

        #region Clean order
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
                    userState.OrderModel.Drinks.Clear();
                    userState.OrderModel.Pizzas.Clear();
                    userState.OrderModel.PriceTotal = 0;
                    userState.OrderModel.QuantityTotal = 0;
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

        #region End Order

        private async Task End_OrderBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    Name = dialogContext.Context.Activity.From.Name,
                    UserIdBot = dialogContext.Context.Activity.From.Id
                };
                context.Users.Add(user);
                context.SaveChanges();
            }

            if (userState.OrderModel.Drinks.Count > 0 || userState.OrderModel.Pizzas.Count > 0)
            {
                if (userState.Address.AddressId == -1)
                {
                    await dialogContext.Begin(AskUserAddressWaterfallText, args);
                }
                else
                {
                    //Todo: Apresentar toda a informação para o usuário
                    //pedido e endereço usando adapative card
                    await dialogContext.Continue();
                }
            }
            else
            {
                await dialogContext.Context.SendActivity($"Você ainda não possui nada em seu carrinho :(");
                await dialogContext.Replace(AskProduct.Ask_Product_Waterfall_Text, args);
            }
        }

        private async Task End_OrderConfirm(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity("Você quer realmente finalizar seu pedido?");
            await dialogContext.Context.SendActivity(MessageFactory.SuggestedActions(
                new CardAction[]
                {
                new CardAction
                {
                    Title = "Sim",
                    Type = ActionTypes.PostBack,
                    Value = ActionTypes.PostBack + "EndOrder||true"
                },
                new CardAction
                {
                    Title = "Nâo",
                    Type = ActionTypes.PostBack,
                    Value = ActionTypes.PostBack + "EndOrder||false"
                }
                })
            );
        }

        private async Task End_OrderEnd(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            Activity activity = (Activity)args["Activity"];
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (activity.Text.Contains(ActionTypes.PostBack + "EndOrder"))
            {
                bool answer = activity.Text.Split("||")[1] == "true" ? true : false;
                if (answer)
                {
                    Order order = new Order
                    {
                        AmmountTotal = userState.OrderModel.PriceTotal,
                        RegisterDate = DateTime.Now,
                        Delivery = userState.Delivery,
                        UsedAddress = context.Addresses.FirstOrDefault(x => x.AddressId == userState.Address.AddressId),
                        User = context.Users.FirstOrDefault(x => x.UserId == user.UserId),
                        OrderDrinks = GetOrderDrinksByOrder(userState.OrderModel.Drinks),
                        OrderPizzas = GetOrderPizzasByOrder(userState.OrderModel.Pizzas),
                        OrderStatus = "Pedido registrado"
                    };

                    context.Orders.Add(order);
                    context.SaveChanges();

                    userState.Address = new Address
                    {
                        AddressId = -1
                    };

                    await dialogContext.Context.SendActivity("Seu pedido foi finalizado Segue as informações dele");
                }
                else
                {
                    await dialogContext.Context.SendActivity("Me diga o que você gostaria agora :)");
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

        #region Ask User Addres Dialog

        private async Task AskUserAddressBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            if (userState.Skip)
            {
                await dialogContext.Continue();
            }
            else
            {
                await dialogContext.Context.SendActivity("Você quer a entrega do pedido?");

                await dialogContext.Context.SendActivity(MessageFactory.SuggestedActions(
                    new CardAction[]
                    {
                        new CardAction
                        {
                            Title = "Sim",
                            Type = ActionTypes.PostBack,
                            Value = ActionTypes.PostBack + "DeliveryOrder||true"
                        },
                        new CardAction
                        {
                            Title = "Nâo",
                            Type = ActionTypes.PostBack,
                            Value = ActionTypes.PostBack + "DeliveryOrder||false"
                        }
                    })
               );
            }
        }

        private async Task AskUserAddressSecondStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();

            if (userState.Skip || !userState.ReuseAddress)
            {
                await dialogContext.Continue();
            }
            else if (user != null)
            {
                userState.UserAddresses = context.Addresses.Where(x => x.UserId == user.UserId).ToList();
                if (userState.UserAddresses.Count > 0)
                {
                    await dialogContext.Continue();
                    //await dialogContext.Replace(ReuseUserAddressWaterfallText, args);
                }
                else
                {
                    await dialogContext.Continue();
                }
            }
            else
            {
                await dialogContext.Continue();
            }
        }

        private async Task AskUserAddressThirdStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            Activity activity = (Activity)args["Activity"];
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);

            if (userState.Skip)
            {
                await dialogContext.Continue();
            }
            else if (activity.Text.Contains(ActionTypes.PostBack + "DeliveryOrder"))
            {
                bool answer = activity.Text.Split("||")[1] == "true" ? true : false;
                if (answer)
                {
                    userState.Delivery = true;
                    await dialogContext.Replace(ReuseUserAddressWaterfallText, args);
                    //await dialogContext.Continue();
                }
                else
                {
                    userState.Delivery = false;
                    await dialogContext.End();
                }
            }
            else
            {
                string text = activity.Text.ToLowerInvariant();
                if (text.Contains("sim"))
                {
                    await dialogContext.Continue();
                }
                else if (text.Contains("nao"))
                {
                    await dialogContext.End();
                }
                else
                {
                    await dialogContext.Context.SendActivity("Não consegui entender tua resposta :( \n (Preferencialmente use os botões)");
                    await dialogContext.Replace(AskUserAddressWaterfallText, args);
                }
            }
        }

        private async Task AskUserAddressFourthStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            await dialogContext.Prompt(TextPrompt, "Qual a rua da entrega?");
        }

        private async Task AskUserAddressFifthStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Address.Street = args["Value"].ToString();

            await dialogContext.Prompt(TextPrompt, "Qual o bairro da entrega?");
        }

        private async Task AskUserAddressSixthStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            userState.Address.Neighborhood = args["Value"].ToString();
            await dialogContext.Prompt(TextPrompt, "Qual o numero do imóvel?");
        }
        private async Task AskUserAddressSeventhStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();
            userState.Address = new Address
            {
                Neighborhood = userState.Address.Neighborhood,
                Street = userState.Address.Street,
                Number = int.Parse(args["Value"].ToString()),
                UserId = user.UserId
            };

            context.Addresses.Add(userState.Address);
            context.SaveChanges();

            await dialogContext.Continue();
        }

        #endregion

        #region Reuse Address

        private async Task ReuseUserAddressBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();
            var addresses = context.Orders.Where(x => x.UsedAddress.UserId == user.UserId).Select(x => new { x.UsedAddress, x.RegisterDate }).ToList();

            foreach (var address in userState.UserAddresses)
            {
                var addressFind = addresses.Where(x => x.UsedAddress == address).FirstOrDefault();
                if (addressFind != null)
                {
                    address.LastUsedDate = addressFind.RegisterDate;
                }
            }
            List<Attachment> attachments = GetUserAddressesAsAttachments(userState.UserAddresses.OrderBy(x => x.LastUsedDate).Take(10).ToList());

            IActivity activity = MessageFactory.Carousel(attachments);

            await dialogContext.Context.SendActivity("Clique no endereço que deseja utilizar, caso não encontre o que deseja utilize a última opção :)");
            await dialogContext.Context.SendActivity(activity);
        }

        private async Task ReuseUserAddressSecondStep(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            string text = dialogContext.Context.Activity.Text;

            if (text.Contains(ActionTypes.PostBack + "UserAddress"))
            {
                int addressId = int.Parse(text.Split("||")[1]);
                userState.Address = context.Addresses.FirstOrDefault(x =>x.AddressId == addressId);
                await dialogContext.Continue();
            }
            else if (text.Contains(ActionTypes.PostBack + "NewAddress"))
            {
                userState.ReuseAddress = false;
                userState.Skip = true;
                await dialogContext.Replace(AskUserAddressWaterfallText, args);
            }
            else
            {
                await dialogContext.Context.SendActivity("Não consegui entender tua resposta :( \n (selecione uma opção)");
                await dialogContext.Replace(ReuseUserAddressWaterfallText, args);
            }
        }

        #endregion


        #region Private Methods

        private ReceiptCard GetReceiptCart(BotUserState userState)
        {
            List<ReceiptItem> receiptItems = new List<ReceiptItem>();
            foreach (var item in userState.OrderModel.Pizzas)
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

            foreach (var item in userState.OrderModel.Drinks)
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
                Total = "R$ " + userState.OrderModel.PriceTotal.ToString("F"),
                Items = receiptItems
            };

            return receiptCard;
        }

        private List<Attachment> GetUserAddressesAsAttachments(List<Address> addresses)
        {
            List<Attachment> attachments = new List<Attachment>();

            foreach (var address in addresses)
            {
                attachments.Add(new HeroCard
                {
                    Title = $"{address.Street} N° {address.Street}",
                    Subtitle = address.Neighborhood,
                    Tap = new CardAction
                    {
                        Type = ActionTypes.PostBack,
                        Value = ActionTypes.PostBack + "UserAddress||" + address.AddressId
                    }
                }.ToAttachment());
            }

            attachments.Add(new HeroCard
            {
                Title = "Adicionar novo endereço",
                Tap = new CardAction
                {
                    Type = ActionTypes.PostBack,
                    Value = ActionTypes.PostBack + "NewAddress"
                }
            }.ToAttachment());

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

        private List<OrderDrink> GetOrderDrinksByOrder(List<DrinkModel> drinks)
        {
            List<OrderDrink> orderDrinks = new List<OrderDrink>();
            foreach (var drink in drinks)
            {
                OrderDrink orderDrinkFind = orderDrinks.FirstOrDefault(x => x.Drink.DrinkId == drink.DrinkId);
                if (orderDrinkFind != null)
                {
                    orderDrinkFind.OrderDrinkSizes.Add(new OrderDrinkSize
                    {
                        DrinkSize = context.SizesD.FirstOrDefault(x => x.SizeName == drink.DrinkSizeName),
                        Quantity = drink.Quantity,
                        Price = drink.Price
                    });
                }
                else
                {
                    OrderDrink orderDrink = new OrderDrink
                    {
                        Drink = context.Drinks.FirstOrDefault(x => x.DrinkId == drink.DrinkId),
                        OrderDrinkSizes = new List<OrderDrinkSize>
                        {
                            new OrderDrinkSize
                            {
                                DrinkSize = context.SizesD.FirstOrDefault(x => x.SizeName == drink.DrinkSizeName),
                                Quantity = drink.Quantity,
                                Price = drink.Price
                            }
                        },
                        DrinkSizeName = drink.DrinkSizeName
                    };
                    orderDrinks.Add(orderDrink);
                }
            }
            return orderDrinks;
        }

        private List<OrderPizza> GetOrderPizzasByOrder(List<PizzaModel> pizzas)
        {
            List<OrderPizza> orderPizzas = new List<OrderPizza>();
            foreach (var pizza in pizzas)
            {
                OrderPizza orderPizzaFind = orderPizzas.FirstOrDefault(x => x.Pizza.PizzaId == pizza.PizzaId);
                if(orderPizzaFind != null)
                {
                    orderPizzaFind.OrderPizzaSizes.Add(new OrderPizzaSize
                    {
                        PizzaSize = context.SizesP.FirstOrDefault(x => x.SizePId == pizza.PizzaSizeId),
                        Quantity = pizza.Quantity,
                        Price = pizza.Price
                    });
                }
                else
                {
                    OrderPizza orderPizza = new OrderPizza
                    {
                        Pizza = context.Pizzas.FirstOrDefault(x => x.PizzaId == pizza.PizzaId),
                        OrderPizzaSizes = new List<OrderPizzaSize>
                        {
                            new OrderPizzaSize
                            {
                                PizzaSize = context.SizesP.FirstOrDefault(x => x.SizePId == pizza.PizzaSizeId),
                                Quantity = pizza.Quantity,
                                Price = pizza.Price
                            }
                        },
                        PizzaSizeName = pizza.SizeName
                    };
                    orderPizzas.Add(orderPizza);
                }
                
            }
            return orderPizzas;
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

        public WaterfallStep[] End_OrderWaterfall()
        {
            return new WaterfallStep[]
            {
                End_OrderBegin,
                End_OrderConfirm,
                End_OrderEnd
            };
        }

        public WaterfallStep[] AskUserAddressWaterfall()
        {
            return new WaterfallStep[]
            {
                AskUserAddressBegin,
                AskUserAddressSecondStep,
                AskUserAddressThirdStep,
                AskUserAddressFourthStep,
                AskUserAddressFifthStep,
                AskUserAddressSixthStep,
                AskUserAddressSeventhStep
            };
        }

        public WaterfallStep[] ReuseUserAddressWaterfall()
        {
            return new WaterfallStep[]
            {
                ReuseUserAddressBegin,
                ReuseUserAddressSecondStep
            };
        }

        #endregion
    }
}
