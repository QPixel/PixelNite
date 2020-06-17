using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ModniteServer.API.Controllers
{
    internal sealed class ChannelsController : Controller
    {
        [Route("GET", "/api/v1/user/setting")]
        public void GetUserSetting()
        {
            var response = new
            {};
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}
