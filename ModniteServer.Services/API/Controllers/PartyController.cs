using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace ModniteServer.API.Controllers
{
    internal sealed class PartyController : Controller
    {
        [Route("GET", "/party/api/v1/Fortnite/user/*")]
        public void GetPartyForUser()
        {
            var response = new
            {
                current = new string[0],
                pending = new string[0],
                invites = new string[0],
                pings = new string[0]
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        [Route("GET", "/party/api/v1/Fortnite/user/*/notifications/undelivered/count")]
        public void GetPartyNotifcations()
        {
            Response.StatusCode = 204;
        }
    }
}
