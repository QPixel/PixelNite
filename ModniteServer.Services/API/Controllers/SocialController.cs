using System;
using System.Collections.Generic;
using System.Text;

namespace ModniteServer.API.Controllers
{
    internal sealed class SocialController : Controller
    {
        [Route("Get","/socialban/api/public/v1/*")]
        public void GetSocialBan()
        {
            Response.StatusCode = 204;

        }
    }
}
