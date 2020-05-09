using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcGames.Models
{
    public class LocationGamesView
    {
        public List<Game> Games { get; set; }
        public List<Location> Location { get; set; }
        public SelectList Locations { get; set; }
        public SelectList Genre { get; set; }
        public string GameGenre { get; set; }
        public string LocationCity { get; set; }
        public string SearchString { get; set; }
    }
}
