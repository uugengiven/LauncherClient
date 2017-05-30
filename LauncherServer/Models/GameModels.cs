﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LauncherServer.Models
{
    public class Game
    {
        public int id { get; set; }
        public int steamId { get; set; }
        public string name { get; set; }
        public string exe { get; set; }
        public virtual List<SteamUser> users { get; set; }
    }

    public class SteamUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool inUse { get; set; }
        public string inUseBy { get; set; }
        public virtual List<Game> games { get; set; }

    }

    public class LauncherDbContext : DbContext
    {
        public LauncherDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<SteamUser> SteamUsers { get; set; }
        public DbSet<Game> Games { get; set; }
    }

    public class GameStartViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string exe { get; set; }
        public int steamId { get; set; }
    }
}