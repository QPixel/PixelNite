using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ModniteServer.API.Controllers
{
    internal sealed class ContentController : Controller
    {
        [Route("GET", "/content/api/pages/fortnite-game/")]
        public void GetNews()
        {
            var response = new
            {
                _title = "Fortnite Game",
                _activeDate = DateTime.Now.AddDays(-1).ToDateTimeString(),
                lastModified = DateTime.Now.ToDateTimeString(),
                _locale = "en-US",
                battleroyalnews = new {
                    news = new
                    { 
                      motd = new List<object>
                      {
                          new
                          {
                            entryType = "Text",
                            image = "https://cdn.discordapp.com/attachments/713173890859270276/713931126564978788/gradient-1920x1080.png",
                            tileImage = "",
                            hidden = false,
                            tabTitleOverride = "PixelNite",
                            _type = "CommonUI Simple Message MOTD",
                            title = "Pixelnite Dev 1.0",
                            body = "This is a test of PixelNite, a new private server by @PixelLeaks_. Credit to Cyuubi (@uguuNatalie) for the SSL Bypass",
                            videoStreamingEnabled = false,
                            sortingPriority = 20,
                            id = "PixelNite-Dev-Welcome",
                            videoFullscreen = false,
                            spotlight = false
                          }
                      }
                    },
                   
            },
                emergencynotice = new
                {
                    news = new
                    {
                        _type = "Battle Royale News",
                        messages = new List<object>()
                    },
                    _title = "emergencynotice",
                    _activeDate = DateTime.Now.AddDays(-1).ToDateTimeString(),
                    lastModified = DateTime.Now.ToDateTimeString(),
                    _locale = "en-US"
                }
            };
            
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(System.IO.File.ReadAllText("Assets/News.json"));
        }
    }
}
