using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcGames.Models;

namespace MvcGames.Data
{
    public class MvcGameContext : DbContext
    {
        public MvcGameContext(DbContextOptions<MvcGameContext> options) : base(options) { }

        public DbSet<Location> Location { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}
