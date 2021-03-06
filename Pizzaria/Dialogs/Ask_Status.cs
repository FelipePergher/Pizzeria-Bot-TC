﻿using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.OrderModels;
using Pizzaria.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class Ask_Status : DialogSet
    {
        public const string AskStatusText = "Ask_Status";
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;
        private readonly string ServerUrl;

        public Ask_Status()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
        }

        #region Ask Status Dialog

        private async Task Ask_StatusBegin(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity(new Activity
            {
                Type = ActivityTypes.Typing
            });

            BotUserState userState = UserState<BotUserState>.Get(dialogContext.Context);
            User user = context.Users.Where(x => x.UserIdBot == dialogContext.Context.Activity.From.Id).FirstOrDefault();

            List<Order> orders = context.Orders.Where(x => x.User.UserId == user.UserId).ToList();
            if(orders.Count > 0)
            {
                IActivity activity = MessageFactory.Carousel(GetAttachmentsByOrders(orders));
                await dialogContext.Context.SendActivity(activity);
            }
            else
            {
                await dialogContext.Context.SendActivity("Você não possui nenhum pedido em aberto no momento!  \n O que você gostaria hoje?");
            }
        }

        #endregion

        #region Private Methods

        private List<Attachment> GetAttachmentsByOrders(List<Order> orders)
        {
            List<Attachment> attachments = new List<Attachment>();

            foreach (var order in orders)
            {
                attachments.Add(new HeroCard
                {
                    Title = "Pedido número " + order.OrderId,
                    Subtitle = "Criado: " + order.RegisterDate.AddHours(-3).ToString("dd/MM/yyyy HH:ss") + " ",
                    Text = " Status: " + order.OrderStatus
                }.ToAttachment());
            }

            return attachments;
        }

        #endregion

        #region Waterfall

        public WaterfallStep[] Ask_StatusWaterfall()
        {
            return new WaterfallStep[]
            {
                Ask_StatusBegin
            };
        }

        #endregion
    }
}
