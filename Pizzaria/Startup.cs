using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pizzaria.Data.Models;
using Pizzaria.Dialogs;

namespace Pizzaria
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBot<Bot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);

                options.Middleware.Add(new CatchExceptionMiddleware<Exception>(async (context, exception) =>
                {
                    await context.TraceActivity("Exception", exception);
                    await context.SendActivity("Desculpe, Alguma coisa deu errado!");
                }));

                IStorage dataStore = new MemoryStorage();

                options.Middleware.Add(new ConversationState<Dictionary<string, object>>(dataStore));


                var (modelId, subscriptionKey, url) = GetLuisConfiguration(Configuration);
                var model = new LuisModel(modelId, subscriptionKey, url);
                options.Middleware.Add(new LuisRecognizerMiddleware(model));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ApplyMigrations(app.ApplicationServices);

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }

        private void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        private (string modelId, string subscriptionKey, Uri url) GetLuisConfiguration(IConfiguration configuration)
        {
            var modelId = configuration.GetSection("Luis-ModelId")?.Value;
            var subscriptionKey = configuration.GetSection("Luis-SubscriptionId")?.Value;
            var url = configuration.GetSection("Luis-Url")?.Value;
            return (modelId, subscriptionKey, new Uri(url));
        }

    }
}
