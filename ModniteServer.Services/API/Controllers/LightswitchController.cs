using Newtonsoft.Json;

namespace ModniteServer.API.Controllers
{
    internal sealed class LightswitchController : Controller
    {
        /// <summary>
        /// Checks the status of Fortnite services. In our case, we're always up when the server is. ;)
        /// </summary>
        [Route("GET", "/lightswitch/api/service/bulk/status")]
        public void GetStatus()
        {
            var response = new[]
            {
                new
                {
                    serviceInstanceId = "fortnite",
                    status = "UP",
                    message = "Down for maintenance",
                    maintenanceUrl = (string)null,
                    overrideCatalogIds = new string[1] { "a7f138b2e51945ffbfdacc1af0541053" },
                    allowedActions = new string[2] { 
                        "PLAY",
                        "DOWNLOAD"
                    },
                    banned = false, // we check for this in OAuthController
                    launcherInfoDTO = new
                    {
                        appName = "Fortnite",
                        catalogItemId = "4fe75bbc5a674f4f9b356b5c90567da5",
                        @namespace = "fn"
                    }
                }
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}