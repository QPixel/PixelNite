using System;
using System.Collections.Generic;
using System.Text;

namespace ModniteServer.API.Controllers
{
    internal sealed class MCPController : Controller
    {
        [Route("POST", "/fortnite/api/game/v2/profileToken/verify/*")]
        public void profileToken()
        {
            Response.StatusCode = 204;
        }
    }
}
