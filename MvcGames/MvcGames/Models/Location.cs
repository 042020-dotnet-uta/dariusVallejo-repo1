using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcGames.Models
{
    public class Location
    {
        public int locationID { get; set; }
        public string City { get; set; }
        public List<Game> inventory { get; set; }
    }
}
