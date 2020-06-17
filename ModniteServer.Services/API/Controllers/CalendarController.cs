﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ModniteServer.API.Controllers
{
    internal sealed class CalendarController : Controller
    {
        [Route("GET", "/fortnite/api/calendar/v1/timeline")]
        public void GetTimeline()
        {
            if (!Authorize()) { }

            // Events never expire in Modnite Server. The start/end dates are always relative to today.
            var clientEvents = new List<object>();
            foreach (string evt in ApiConfig.Current.ClientEvents)
            {
                clientEvents.Add(new
                {
                    eventType = "EventFlag." + evt,
                    activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                    activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
                });
            }
            clientEvents.Add(new
            {
                eventType = "FEC01",
                activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
            });
            clientEvents.Add(new
            {
                eventType = "FEC02",
                activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
            });
            clientEvents.Add(new
            {
                eventType = "FEC03",
                activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
            });
            clientEvents.Add(new
            {
                eventType = "FLA01",
                activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
            });
            clientEvents.Add(new
            {
                eventType = "FLCD01",
                activeUntil = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                activeSince = DateTime.Now.Date.AddDays(-1).ToDateTimeString()
            });
            var response = new
            {
                channels = new Dictionary<string, object>
                {
                    ["standalone-store"] = new {},
                    ["client-matchmaking"] = new {},
                    ["tk"] = new {},
                    ["feature-islands"] = new {},
                    ["community-votes"] = new {},
                    ["client-events"] = new
                    {
                        // You can set event flags in config.json. Visit https://modnite.net for a list of event flags.
                        // Event flags control stuff like lobby theme, battle bus skin, etc.
                        states = new[]
                        {
                            new
                            {
                                validFrom = DateTime.Now.AddDays(-1).ToDateTimeString(),
                                activeEvents = clientEvents,
                                state = new
                                {
                                    activeStorefronts = new string[0],
                                    eventNamedWeights = new { },
                                    activeEvents = new string[0],
                                    seasonNumber = ApiConfig.Current.Season,
                                    seasonTemplateId = "AthenaSeason:athenaseason" + ApiConfig.Current.Season,
                                    matchXpBonusPoints = 0, // Bonus XP Event
                                    eventPunchCardTemplateId = "",
                                    seasonBegin = DateTime.Now.Date.AddDays(-30).ToDateTimeString(),
                                    seasonEnd = DateTime.Now.Date.AddDays(30).ToDateTimeString(),
                                    seasonDisplayedEnd = DateTime.Now.Date.AddDays(30).ToDateTimeString(),
                                    weeklyStoreEnd = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                                    stwEventStoreEnd = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                                    stwWeeklyStoreEnd = DateTime.Now.Date.AddDays(7).ToDateTimeString(),
                                    dailyStoreEnd = DateTime.Now.Date.AddDays(7).ToDateTimeString()
                                }
                            }
                        },
                        cacheExpire = DateTime.Now.Date.AddDays(7).ToDateTimeString()
                    }
                },
                cacheIntervalMins = 15,
                currentTime = DateTime.Now.ToDateTimeString()
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}
