using System;
using System.Collections.Generic;
using System.Text;

namespace ModniteServer.API.Controllers
{
   internal sealed class PresenceController : Controller
    {
        [Route("GET", "/presence/api/v1/_/*/settings/subscriptions")]
        public void GetPresenceSubscriptions()
        {
            Response.StatusCode = 204;
        }

        [Route("POST", "/presence/api/v1/Fortnite/imqpixel/subscriptions/broadcast")]
        public void GetPresenceBroadcast()
        {
            Response.StatusCode = 204;
        }
    }
}
