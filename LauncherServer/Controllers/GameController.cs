﻿using LauncherServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jose;
using System.Security.Cryptography;

namespace LauncherServer.Controllers
{
    public class GameController : Controller
    {
        LauncherDbContext db = new LauncherDbContext();
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult StartGame(int id = 0)
        {
            if (id > 0)
            {

                var game = db.Games.Where(x => x.steamId == id).First();
                var user = db.SteamUsers.Where(x => x.games.Any(g => g.id == game.id) && x.inUse == false).FirstOrDefault();
                if (user != null)
                {
                    var output = new GameStartViewModel();
                    output.exe = game.exe;
                    output.steamId = game.steamId;
                    output.username = user.username;
                    output.password = Decrypt(user.password);
                    user.inUse = true;
                    user.inUseBy = db.Computers.Find(1);
                    db.SaveChanges();
                    return new JsonResult() { Data = output, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                    return new JsonResult() { Data = "no users available", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = "more poop", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public string Setup()
        {
            var game = new Game();
            //game.exe = "dota2.exe";
            //game.name = "Dota 2";
            //game.steamId = 570;
            //game.users = new List<SteamUser>();
            //db.Games.Add(game);
            //db.SaveChanges();

            //var su = new SteamUser();
            //su.username = "SomeDota";
            //su.password = Encrypt("dota2");
            //su.games = new List<Game>();
            //su.games.Add(game);
            //db.SteamUsers.Add(su);

            //var su1 = new SteamUser();
            //su1.username = "LFGDeadbydaylight02";
            //su1.password = Encrypt("asdf");
            //su1.games = new List<Game>();
            //db.SteamUsers.Add(su1);


            //var game = db.Games.Find(3);
            //game.name = "Dead by Daylight";
            //game.steamId = 381210;
            //game.exe = "deadbydaylight.exe";
            //game.users = new List<SteamUser>();
            //db.Games.Add(game);
            //db.SaveChanges();

            //su.games.Add(game);
            //su1.games.Add(game);
            //db.SaveChanges();

            foreach (var u in db.SteamUsers)
            {
                u.inUse = false;
            }
            db.SaveChanges();

            return "ok";
        }

        public string Encrypt(string value)
        {
            var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
            return Jose.JWT.Encode(value, secretKey, JwsAlgorithm.HS256);
        }

        public string Decrypt(string value)
        {
            var secretKeybit = "pDzCAKG9KSaCWY2kLaqf0UWJ89i/gy/6IGvndSWe4eo=";
            var secretKey = System.Convert.FromBase64String(secretKeybit);
            return Jose.JWT.Decode(value, secretKey, JwsAlgorithm.HS256);
        }

        public string getSalt(int max_length)
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[max_length];
            random.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

    }
}