using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pizzaria.Code;
using Pizzaria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Dialogs
{
    public class AskProduct
    {
        public const string Ask_ProductText = "Ask_Product";
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;
        private readonly string ServerUrl;

        public AskProduct()
        {
            Configuration = Startup.ConfigurationStatic;
            ServerUrl = Configuration.GetSection("ServerUrl").Value;
            //ServerUrl = dialogContext.Context.Activity.ServiceUrl;
            context = ServiceProviderFactory.GetApplicationDbContext();
        }
       
        public async Task Ask_Product(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            var pizzas = context.Pizzas;
            IList<Attachment> heroCardsAttachments = new List<Attachment>();


            foreach (var pizza in pizzas)
            {
                heroCardsAttachments.Add(
                    new HeroCard
                    {
                        Title = pizza.Name,
                        Subtitle = pizza.PizzaType,
                        Images = new CardImage[] { new CardImage(url: ServerUrl + @"/" + pizza.Image) }
                    }.ToAttachment());
            }


            IMessageActivity activity = MessageFactory.Attachment(heroCardsAttachments);
            activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            await dialogContext.Context.SendActivities(new[] { activity });
        }

        public async Task Teste(DialogContext dialogContext, IDictionary<string, object> args, SkipStepFunction next)
        {
            await dialogContext.Context.SendActivity(args.ToString());
        }

        public WaterfallStep[] Ask_ProductWaterfall()
        {
            return new WaterfallStep[]
            {
                Ask_Product,
                Teste
            };
        }
    }
}
