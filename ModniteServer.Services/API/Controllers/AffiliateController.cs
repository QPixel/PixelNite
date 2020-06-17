using System.Linq;
using Newtonsoft.Json;
namespace ModniteServer.API.Controllers
{
    internal sealed class AffiliateController : Controller
    {
        [Route("GET", "/affiliate/api/public/affiliates/slug/*")]
        public void CheckIfAffiliateExists()
        {
            string affiliateName = Request.Url.Segments.Last();

            var response = new
            {
                id = "ThisisStupid",
                slug = "PxlLeaks",
                displayName = "PixelLeaks",
                status = "Active",
                verified = true
            };
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}