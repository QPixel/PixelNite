using ModniteServer.API.Accounts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
namespace ModniteServer.API.Controllers
{
    internal sealed class CustomizationController : Controller
    {
        /// <summary>
        /// Updates the player's equipped item choice.
        /// </summary>
        [Route("POST", "/fortnite/api/game/v2/profile/*/client/SetCosmeticLockerSlot")]
        public void EquipBattleRoyaleCustomization()
        {
            string accountId = Request.Url.Segments[Request.Url.Segments.Length - 3].Replace("/", "");

            string profileId = Query["profileId"];
            int rvn = Convert.ToInt32(Query["rvn"]);

            byte[] buffer = new byte[Request.ContentLength64];
            Request.InputStream.Read(buffer, 0, buffer.Length);

            var request = JObject.Parse(Encoding.UTF8.GetString(buffer));
            string lockeritem = (string)request["lockerItem"];
            string slotName = (string)request["category"];
            string itemToSlot = (string)request["itemToSlot"];
            int indexWithinSlot = (int)request["slotIndex"];
            Account account = AccountManager.GetAccount(accountId);


            switch (slotName)
            {
                case "Character":
                    account.EquippedItems["favorite_character"] = itemToSlot;
                    break;
                case "Dance":
                    string dance = "favorite_dance" + indexWithinSlot;
                    account.EquippedItems[dance] = itemToSlot;
                    break;
                case "Backpack":
                    account.EquippedItems["favorite_backpack"] = itemToSlot;
                    break;
                case "Pickaxe":
                    account.EquippedItems["favorite_pickaxe"] = itemToSlot;
                    break;
                case "Glider":
                    account.EquippedItems["favorite_glider"] = itemToSlot;
                    break;
                case "SkyDiveContrail":
                    account.EquippedItems["favorite_skydivecontrail"] = itemToSlot;
                    break;
                case "MusicPack":
                    account.EquippedItems["favorite_musicpack"] = itemToSlot;
                    break;
                case "LoadingScreen":
                    account.EquippedItems["favorite_loadingscreen"] = itemToSlot;
                    break;
                default:
                    break;
                    
            }
            
            string Character = account.EquippedItems["favorite_character"];
            string[] Dance = new string[6];
            for (int i =0; i < Dance.Length; i++)
            {
                Dance[i] = account.EquippedItems["favorite_dance" + i];
            }
            string[] ItemWrap = new string[7];
            for (int i = 0; i < ItemWrap.Length; i++)
            {
                ItemWrap[i] = "";
            }
            string Glider = string.IsNullOrEmpty(account.EquippedItems["favorite_glider"]) ? null : account.EquippedItems["favorite_glider"];
            string Pickaxe = string.IsNullOrEmpty(account.EquippedItems["favorite_pickaxe"]) ? null : account.EquippedItems["favorite_pickaxe"];
            string Backpack = string.IsNullOrEmpty(account.EquippedItems["favorite_backpack"]) ? null : account.EquippedItems["favorite_backpack"];
            string LoadingScreen = string.IsNullOrEmpty(account.EquippedItems["favorite_loadingscreen"]) ? null : account.EquippedItems["favorite_loadingscreen"];
            string MusicPack = string.IsNullOrEmpty(account.EquippedItems["favorite_musicpack"]) ? null : account.EquippedItems["favorite_musicpack"];
            string SkyDiveContrail = string.IsNullOrEmpty(account.EquippedItems["favorite_skydivecontrail"]) ? null : account.EquippedItems["favorite_skydivecontrail"];
            var response = new
            {
                profileRevision = rvn + 1,
                profileId,
                profileChangesBaseRevision = rvn,
                profileChanges = new List<object>
                {
                    new
                    {
                        changeType = "itemAttrChanged",
                        attributeName = "locker_slots_data",
                        itemId = lockeritem,
                        attributeValue = new
                        {
                            slots = new
                            {
                                Glider = new {
                                    items = new List<object>
                                    {
                                        Glider
                                    }
                                },
                                Dance = new
                                {
                                    items = Dance,
                                    activeVariants = new string[6]
                                },
                                SkyDiveContrail = new
                                {
                                    items = new List<object>
                                    {
                                        SkyDiveContrail
                                    }
                                },
                                LoadingScreen = new
                                {
                                    items = new List<object>
                                    {
                                        LoadingScreen
                                    }
                                },
                                Pickaxe = new
                                {
                                    items = new List<object>
                                    {
                                        Pickaxe
                                    }
                                },
                                ItemWrap = new
                                {
                                    items = ItemWrap
                                },
                                MusicPack = new
                                {
                                    items = new List<object>
                                    {
                                        MusicPack
                                    }
                                },
                                Character = new
                                {
                                    items = new List<object>
                                    {
                                        Character
                                    },
                                    activeVariants = new List<object>
                                    {
                                       new { }
                                    }
                                },
                                Backpack = new
                                {
                                    items = new List<object>
                                    {
                                        Backpack
                                    }
                                }

                            }
                        }
                    }
                },
                serverTime = DateTime.Now.ToDateTimeString(),
                profileCommandRevision = rvn + 1,
                responseVersion = 1
            };

            Debug.WriteLine($"{response}");

            // TODO: update in Account model
            Log.Information($"'{accountId}' changed favorite {slotName}");
            
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Updates the player's banner.
        /// </summary>
        [Route("POST", "/fortnite/api/game/v2/profile/*/client/SetBattleRoyaleBanner")]
        public void EquipBanner()
        {
            string accountId = Request.Url.Segments[Request.Url.Segments.Length - 3].Replace("/", "");

            string profileId = Query["profileId"];
            int rvn = Convert.ToInt32(Query["rvn"]);

            byte[] buffer = new byte[Request.ContentLength64];
            Request.InputStream.Read(buffer, 0, buffer.Length);

            var request = JObject.Parse(Encoding.UTF8.GetString(buffer));

            // TODO: return entire athena profile data
            Response.StatusCode = 500;
        }
    }
}