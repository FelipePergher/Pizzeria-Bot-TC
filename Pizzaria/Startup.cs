﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Pizzaria.Code;
using Pizzaria.Data.Models;
using Pizzaria.Data.Models.DrinkModels;
using Pizzaria.Data.Models.PizzaModels;
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
            Startup.ConfigurationStatic = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration ConfigurationStatic { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBot<Bot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);

                options.Middleware.Add(new CatchExceptionMiddleware<Exception>(async (context, exception) =>
                {
                    await context.SendActivity($"Olá, ainda não estou preparado para tratar este tipo de informacão {Emojis.SmileSad}  \n" +
                    $"Peço que utilize apenas texto para melhorar nossa interação {Emojis.SmileHappy}");
                }));

                IStorage dataStore = new MemoryStorage();
                options.Middleware.Add(new ConversationState<Dictionary<string, object>>(dataStore));
                options.Middleware.Add(new UserState<BotUserState>(dataStore));


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
            SeedData(app.ApplicationServices);

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

        private void SeedData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (!context.SizesD.Any())
                    {
                        List<SizeD> sizesD = new List<SizeD>
                        {
                            new SizeD
                            {
                                Quantity = InformationName.Lata350,
                                SizeName = InformationName.Lata350Name
                            },
                            new SizeD
                            {
                                Quantity = InformationName.Garrafa500,
                                SizeName = InformationName.Garrafa500Name
                            },
                            new SizeD
                            {
                                Quantity = InformationName.Garrafa600,
                                SizeName = InformationName.Garrafa600Name
                            },
                            new SizeD
                            {
                                Quantity = InformationName.Litro01,
                                SizeName = InformationName.Litro01Name
                            },
                            new SizeD
                            {
                                Quantity = InformationName.Litro02,
                                SizeName = InformationName.Litro02Name
                            },
                            new SizeD
                            {
                                Quantity = InformationName.Litro15,
                                SizeName = InformationName.Litro15Name
                            }
                        };
                        List<Drink> drinks = new List<Drink>
                        {
                            new Drink
                            {
                                Name = "Coca-Cola",
                                Image = "Images/Drinks/coca.png",
                                DrinkSizes = new List<DrinkSize>
                                {
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Lata350).FirstOrDefault(),
                                        Price = 3.50d
                                    },
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Garrafa600).FirstOrDefault(),
                                        Price = 5.00d
                                    },
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Litro02).FirstOrDefault(),
                                        Price = 7.50d
                                    }
                                }
                            },
                            new Drink
                            {
                                Name = "Sprite",
                                Image = "Images/Drinks/sprite.jpg",
                                DrinkSizes = new List<DrinkSize>
                                {
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Lata350).FirstOrDefault(),
                                        Price = 3.50d
                                    },
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Garrafa600).FirstOrDefault(),
                                        Price = 5.00d
                                    },
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Litro02).FirstOrDefault(),
                                        Price = 7.50d
                                    },
                                }
                            },
                            new Drink
                            {
                                Name = "Água Mineral",
                                Image = "Images/Drinks/agua.png",
                                DrinkSizes = new List<DrinkSize>
                                {
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Garrafa500).FirstOrDefault(),
                                        Price = 3.00d
                                    },
                                    new DrinkSize
                                    {
                                        SizeD = sizesD.Where(x => x.Quantity == InformationName.Litro15).FirstOrDefault(),
                                        Price = 5.00d
                                    },
                                }
                            }
                        };

                        List<Ingredient> ingredients = new List<Ingredient>
                        {
                            new Ingredient
                            {
                                Name = "milho"
                            },
                            new Ingredient
                            {
                                Name = "alho"
                            },
                            new Ingredient
                            {
                                Name = "atum"
                            },
                            new Ingredient
                            {
                                Name = "azeitona"
                            },
                            new Ingredient
                            {
                                Name = "bacon"
                            },
                            new Ingredient
                            {
                                Name = "banana"
                            },
                            new Ingredient
                            {
                                Name = "calabresa"
                            },
                            new Ingredient
                            {
                                Name = "carne"
                            },
                            new Ingredient
                            {
                                Name = "catupiry"
                            },
                            new Ingredient
                            {
                                Name = "cebola"
                            },
                            new Ingredient
                            {
                                Name = "champignon"
                            },
                            new Ingredient
                            {
                                Name = "cheddar"
                            },
                            new Ingredient
                            {
                                Name = "cream cheese"
                            },
                            new Ingredient
                            {
                                Name = "ervilha"
                            },
                            new Ingredient
                            {
                                Name = "frango"
                            },
                            new Ingredient
                            {
                                Name = "gorgonzola"
                            },
                            new Ingredient
                            {
                                Name = "lombo"
                            },
                            new Ingredient
                            {
                                Name = "mussarela"
                            },
                            new Ingredient
                            {
                                Name = "orégano"
                            },
                            new Ingredient
                            {
                                Name = "ovo"
                            },
                            new Ingredient
                            {
                                Name = "palmito"
                            },
                            new Ingredient
                            {
                                Name = "parmesão"
                            },
                            new Ingredient
                            {
                                Name = "pepino"
                            },
                            new Ingredient
                            {
                                Name = "pimenta"
                            },
                            new Ingredient
                            {
                                Name = "pimentão"
                            },
                            new Ingredient
                            {
                                Name = "presunto"
                            },
                            new Ingredient
                            {
                                Name = "provolone"
                            },
                            new Ingredient
                            {
                                Name = "queijo"
                            },
                            new Ingredient
                            {
                                Name = "requeijão"
                            },
                            new Ingredient
                            {
                                Name = "salame"
                            },
                            new Ingredient
                            {
                                Name = "tomate"
                            },
                            new Ingredient
                            {
                                Name = "molho"
                            },
                            new Ingredient
                            {
                                Name = "manjericão"
                            }
                        };
                        List<SizeP> sizesP = new List<SizeP>
                        {
                            new SizeP
                            {
                                Size = InformationName.Small
                            },
                            new SizeP
                            {
                                Size = InformationName.Medium
                            },
                            new SizeP
                            {
                                Size = InformationName.Large
                            },
                            new SizeP
                            {
                                Size = InformationName.Family
                            }
                        };
                        List<Pizza> pizzas = new List<Pizza>
                        {
                            new Pizza
                            {
                                Name = "Calabresa",
                                Vegetarian = false,
                                PizzaType = InformationName.Salted,
                                Image = "Images/Pizzas/calabresa.png",
                                PizzaSizes = new List<PizzaSize>
                                {
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Family).FirstOrDefault(),
                                        Price = InformationName.FamilyPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Small).FirstOrDefault(),
                                        Price = InformationName.SmallPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Large).FirstOrDefault(),
                                        Price = InformationName.LargePrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Medium).FirstOrDefault(),
                                        Price = InformationName.MediumPrice
                                    }
                                },
                                PizzaIngredients = new List<PizzaIngredient>
                                {
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "molho").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "mussarela").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "calabresa").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "cebola").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "orégano").FirstOrDefault()
                                    }
                                }
                            },
                            new Pizza
                            {
                                Name = "Mussarela",
                                Vegetarian = false,
                                PizzaType = InformationName.Salted,
                                Image = "Images/Pizzas/portuguesa.png",
                                PizzaSizes = new List<PizzaSize>
                                {
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Family).FirstOrDefault(),
                                        Price = InformationName.FamilyPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Small).FirstOrDefault(),
                                        Price = InformationName.SmallPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Large).FirstOrDefault(),
                                        Price = InformationName.LargePrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Medium).FirstOrDefault(),
                                        Price = InformationName.MediumPrice
                                    }
                                },
                                PizzaIngredients = new List<PizzaIngredient>
                                {
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "molho").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "mussarela").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "tomate").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "orégano").FirstOrDefault()
                                    }
                                }
                            },
                            new Pizza
                            {
                                Name = "Margherita",
                                Vegetarian = false,
                                PizzaType = InformationName.Salted,
                                Image = "Images/Pizzas/bacon.png",
                                PizzaSizes = new List<PizzaSize>
                                {
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Family).FirstOrDefault(),
                                        Price = InformationName.FamilyPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Small).FirstOrDefault(),
                                        Price = InformationName.SmallPrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Large).FirstOrDefault(),
                                        Price = InformationName.LargePrice
                                    },
                                    new PizzaSize
                                    {
                                        SizeP = sizesP.Where(x => x.Size == InformationName.Medium).FirstOrDefault(),
                                        Price = InformationName.MediumPrice
                                    }
                                },
                                PizzaIngredients = new List<PizzaIngredient>
                                {
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "molho").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "mussarela").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "tomate").FirstOrDefault()
                                    },
                                    new PizzaIngredient
                                    {
                                        Ingredient = ingredients.Where(x => x.Name.ToLower() == "manjericão").FirstOrDefault()
                                    }
                                }
                            }
                        };

                        context.SizesD.AddRange(sizesD);
                        context.Drinks.AddRange(drinks);
                        context.SaveChanges();

                        context.Ingredients.AddRange(ingredients);
                        context.SizesP.AddRange(sizesP);
                        context.Pizzas.AddRange(pizzas);
                        context.SaveChanges();
                    }
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
