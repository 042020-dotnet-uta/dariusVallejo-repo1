using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcGames.Data;
using System;
using System.Linq;
using MvcGames.Models;

namespace MvcGames.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider) {
            using (var context = new MvcGameContext
                (serviceProvider.GetRequiredService<DbContextOptions<MvcGameContext>>())){
                if (context.Game.Any() && context.Location.Any()) {
                    return;
                }

                if (context.Game.Any() && !context.Location.Any())
                {
                    locationHelper(context);
                }
                else if (!context.Game.Any() && context.Location.Any())
                {
                    gameHelper(context);
                }
                else {
                    locationHelper(context);
                    gameHelper(context);
                }
            }
        }

        static void gameHelper(MvcGameContext context) {
            context.Game.AddRange(
                    new Game
                    {
                        Title = "Annimal Crossing",
                        ReleaseDate = DateTime.Parse("2020-03-20"),
                        Genre = "Simulation",
                        Price = 59.99M,
                        locationID = 1
                    },
                    new Game
                    {
                        Title = "Doom",
                        ReleaseDate = DateTime.Parse("2020-03-20"),
                        Genre = "Action FPS",
                        Price = 59.99M,
                        locationID = 1
                    },
                    new Game
                    {
                        Title = "Pokemon",
                        ReleaseDate = DateTime.Parse("2019-03-20"),
                        Genre = "RPG",
                        Price = 59.99M,
                        locationID = 1
                    },
                    new Game
                    {
                        Title = "P.T.",
                        ReleaseDate = DateTime.Parse("2014-01-20"),
                        Genre = "Horror",
                        Price = 0M,
                        locationID = 2
                    },
                    new Game
                    {
                        Title = "Annimal Crossing",
                        ReleaseDate = DateTime.Parse("2020-03-20"),
                        Genre = "Simulation",
                        Price = 59.99M,
                        locationID = 2
                    },
                    new Game
                    {
                        Title = "The Last of Us",
                        ReleaseDate = DateTime.Parse("2012-03-20"),
                        Genre = "Action Advendture",
                        Price = 59.99M,
                        locationID = 2
                    }
                );
            context.SaveChanges();
        }

        static void locationHelper(MvcGameContext context) {
            context.Location.AddRange(
                    new Location
                    {
                        City = "Dallas"
                    },
                    new Location
                    { 
                        City = "New York"
                    }
                );
            context.SaveChanges();
        }
    }
}
