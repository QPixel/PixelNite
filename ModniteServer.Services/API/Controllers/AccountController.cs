using ModniteServer.API.Accounts;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace ModniteServer.API.Controllers
{
    public class GetMultipleAccounts
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    /// <summary>
    /// Handles requests for retrieving info on accounts.
    /// </summary>
    internal sealed class AccountController : Controller
    {
        ///<summary>
        ///Account Info stuff
        /// </summary>
        [Route("GET", "/account/api/public/account")]
        public void GetPublicAccountInfo()
        {
            Query.TryGetValue("accountId", out string accountId);
            if (!Authorize()) return;
            if (!AccountManager.AccountExists(accountId))
            {
                Response.StatusCode = 404;
                return;
            }
            
            var account = AccountManager.GetAccount(accountId);

            IList<GetMultipleAccounts> Accounts = new List<GetMultipleAccounts>
            {
                new GetMultipleAccounts
                {
                    id = accountId,
                    displayName = account.DisplayName,

                }
            };
            
            JArray response = new JArray(
                Accounts.Select(p => new JObject
                {
                    {"id", p.id },
                    {"displayName", p.displayName },
                    {"externalAuths", new JObject {} }
                }
                ));

            Log.Information($"[AccountController] Account info retrieved for {accountId} {{AccountInfo}}", response.ToString());

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Retrieves basic info for an account.
        /// </summary>
        [Route("GET", "/account/api/public/account/*")]
        public void GetAccountInfo()
        {
            if (!Authorize()) return;

            string accountId = Request.Url.Segments.Last();

            if (!AccountManager.AccountExists(accountId))
            {
                Response.StatusCode = 404;
                return;
            }

            var account = AccountManager.GetAccount(accountId);

            var response = new
            {
                id = account.AccountId,
                displayName = account.DisplayName,
                externalAuths = new {}
            };

            Log.Information($"Account info retrieved for {accountId} {{AccountInfo}}", response);

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
        
        /// <summary>
        /// Gets the linked account info for an account. Since we don't provide the capability of
        /// signing into accounts using Facebook, Google, or whatever, we're just going to provide
        /// the same info as <see cref="GetAccountInfo"/>.
        /// </summary>
        [Route("GET", "/account/api/public/account/*/externalAuths")]
        public void GetExternalAuthInfo()
        {

            //    if (!Authorize()) return;

            //    string accountId = Request.Url.Segments[Request.Url.Segments.Length - 2];

            //    if (!AccountManager.AccountExists(accountId))
            //    {
            //        Response.StatusCode = 404;
            //        return;
            //    }

            //    var account = AccountManager.GetAccount(accountId);

            //    var response = new
            //    {
            //        url = "",
            //        id = account.AccountId,
            //        externalAuthId = account.AccountId,
            //        externalDisplayName = account.DisplayName,
            //        externalId = account.AccountId,
            //        externalauths = "epic",
            // users = new []
            // {
            // new
            // {
            // externalAuthId = account.AccountId,
            // externalDisplayName = account.DisplayName,
            // externalId = account.AccountId
            // }
            // }
            // };
            var response = new { };
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}
