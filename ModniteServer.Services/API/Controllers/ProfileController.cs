using ModniteServer.API.Accounts;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace ModniteServer.API.Controllers
{
    internal sealed class ProfileController : Controller
    {
        private string _accountId;
        private Account _account;
        private int _revision;

        [Route("POST", "/fortnite/api/game/v2/profile/*/client/QueryProfile")]
        public void QueryProfile()
        {
            _accountId = Request.Url.Segments[Request.Url.Segments.Length - 3].Replace("/", "");

            if (!AccountManager.AccountExists(_accountId))
            {
                Response.StatusCode = 404;
                return;
            }

            _account = AccountManager.GetAccount(_accountId);

            Query.TryGetValue("profileId", out string profileId);
            Query.TryGetValue("rvn", out string rvn);
            _revision = Convert.ToInt32(rvn ?? "-2");

            switch (profileId)
            {
                case "common_core":
                    QueryCommonCoreProfile();
                    break;

                case "common_public":
                    QueryCommonPublicProfile();
                    break;

                case "athena":
                    QueryAthenaProfile();
                    break;

                case "creative":
                    QueryCreativeProfile();
                    break;

                case "collections":
                    QueryCollectionsProfile();
                    break;

                default:
                    QueryProfileError(profileId);
                    break;
            }
        }
        private void QueryCollectionsProfile()
        {
            var response = new
            {
                profileRevision = 1,
                profileId = "collections",
                profileChangesBaseRevision = 1,
                profileChanges = new List<object>
                {
                    new
                    {
                        changeType = "fullProfileUpdate",
                        profile = new
                        {
                            _id = _accountId, // not really account id but idk
                            created = DateTime.Now.ToDateTimeString(),
                            updated = DateTime.Now.ToDateTimeString(),
                            rvn = 1,
                            wipeNumber = 1,
                            accountId = _accountId,
                            profileId = "collections",
                            version = "pixelnite_release_1260_may_2020",
                            items = new {},
                            stats = new
                            {
                                attributes = new  {}
                                }
                            },
                            commandRevision = 0
                        }
                    },
                profileCommandRevision = 0,
                serverTime = DateTime.Now.ToDateTimeString(),
                responseVersion = 1
            };
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
            Log.Information("[ProfileController] Retrieved profile 'collections' {AccountId}{Profile}{Revision}", _accountId, response, _revision);
        }
        
        public void QueryCommonCoreProfile()
        {
            var items = _account.CoreItems;

            var itemsFormatted = new Dictionary<string, object>();
            foreach (string item in items)
            {
                itemsFormatted.Add(Guid.NewGuid().ToString(), new
                {
                    templateId = item,
                    attributes = new
                    {
                        item_seen = 1
                    },
                    quantity = 1
                });
            }
            itemsFormatted.Add("Currency:MTXPurchased", new
            {
                attributes = new
                {
                    platform = "EpicPC"
                },
                quantity=1000000,
                templateId="Currency:MtxPurchased"
            });
            var response = new
            {
                profileRevision = 1,
                profileId = "common_core",
                profileChangesBaseRevision = 1,
                profileChanges = new List<object>
                 {
                    new
                    {
                        changeType = "fullProfileUpdate",
                        profile = new
                        {
                            _id = _accountId, // not really account id but idk
                            created = DateTime.Now.ToDateTimeString(),
                            updated = DateTime.Now.ToDateTimeString(),
                            rvn = 1,
                            wipeNumber = 1,
                            accountId = _accountId,
                            profileId = "common_core",
                            version = "pixelnite_release_1260_may_2020",
                            items = itemsFormatted,
                            stats = new
                            {
                                attributes = new
                                {
                                   mfa_enabled = true,
                                   mtx_affiliate = "PxlLeaks",
                                   current_mtx_platform = "EpicPC",
                                   in_app_purchases = new {
                                        receipts = new string[0]
                                   }
                                }
                            },
                        }
                    }
                },
                serverTime = DateTime.Now.ToDateTimeString(),
                profileCommandRevision = 1,
                responseVersion = 1
            };

            Log.Information("[ProfileController] Retrieved profile 'common_core' {AccountId}{Profile}{Revision}", _accountId, response, _revision);

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        private void QueryCommonPublicProfile()
        {
            var response = new
            {
                profileRevision = 1,
                profileId = "common_public",
                profileChangesBaseRevision = 1,
                profileChanges = new List<object>(),
                serverTime = DateTime.Now.ToDateTimeString(),
                profileCommandRevision = 1,
                responseVersion = 1
            };

            Log.Information("[ProfileController] Retrieved profile 'common_public' {AccountId}{Profile}{Revision}", _accountId, response, _revision);

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        private void QueryCreativeProfile()
        {
            // NOTE: Fortnite will not load if creative profile is not set, Idfk why.
            var response = new
            {
                profileRevision = 1,
                profileId = "creative",
                profileChangesBaseRevision = 1,
                profileChanges = new List<object>(),
                serverTime = DateTime.Now.ToDateTimeString(),
                profileCommandRevision = 1,
                responseVersion = 1
            };
            Log.Information("[ProfileController] Retrieved profile 'creative' {AccountId}{Profile}{Revision}", _accountId, response, _revision);

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
        private void QueryProfileError(string profileId)
        {
            var response = new
            {
                errorCode = "errors.com.epicgames.modules.profiles.operation_forbidden",
                errorMessage = $"Unable to find template confiuration for profile {profileId}",
                messageVars = new string [1]
                {
                    profileId
                },
                numericErrorCode = 12813,
                originatingService = "fortnite",
                intent = "prod-live"
            };
            Response.StatusCode = 403;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
        private void QueryAthenaProfile()
        {
            var items = _account.AthenaItems;
            var itemsFormatted = new Dictionary<string, object>();
            string Character = _account.EquippedItems["favorite_character"];
            string[] Dance = new string[6];
            for (int i = 0; i < Dance.Length; i++)
            {
                Dance[i] = _account.EquippedItems["favorite_dance" + i];
            }
            string[] ItemWrap = new string[7];
            for (int i = 0; i < ItemWrap.Length; i++)
            {
                ItemWrap[i] = "";
            }
            string Glider = string.IsNullOrEmpty(_account.EquippedItems["favorite_glider"]) ? null : _account.EquippedItems["favorite_glider"];
            string Pickaxe = string.IsNullOrEmpty(_account.EquippedItems["favorite_pickaxe"]) ? null : _account.EquippedItems["favorite_pickaxe"];
            string Backpack = string.IsNullOrEmpty(_account.EquippedItems["favorite_backpack"]) ? null : _account.EquippedItems["favorite_backpack"];
            string LoadingScreen = string.IsNullOrEmpty(_account.EquippedItems["favorite_loadingscreen"]) ? null : _account.EquippedItems["favorite_loadingscreen"];
            string MusicPack = string.IsNullOrEmpty(_account.EquippedItems["favorite_musicpack"]) ? null : _account.EquippedItems["favorite_musicpack"];
            string SkyDiveContrail = string.IsNullOrEmpty(_account.EquippedItems["favorite_skydivecontrail"]) ? null : _account.EquippedItems["favorite_skydivecontrail"];
            itemsFormatted.Add("CosmeticLocker:cosmeticlocker_athena1", new
            {
                templateId = "CosmeticLocker:cosmeticlocker_athena",
                attributes = new
                {
                    locker_slots_data = new
                    {
                        slots = new
                        {
                            Glider = new
                            {
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
                                       new {}
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
                    },
                    use_count = 0,
                    banner_icon_template = "OtherBanner28",
                    banner_color_template = "defaultcolor0",
                    locker_name = "Test",
                    item_seen = false,
                    favorite = false,
                },
                quantity = 1
            });

            foreach (string item in items)
            {
                var itemGuid = item;
                itemsFormatted.Add(itemGuid, new
                {
                    templateId = itemGuid,

                    attributes = new
                    {
                        max_level_bonus = 0,
                        level = 1,
                        item_seen = true,
                        rnd_sel_cnt = 0,
                        xp = 0,
                        variants = new List<object>(),
                        favorite = false
                    },
                    quantity = 1
                });
            }

            var response = new
            {
                profileRevision = 1,
                profileId = "athena",
                profileChangesBaseRevision = 1,
                profileChanges = new List<object>
                {
                    new
                    {
                        changeType = "fullProfileUpdate",
                        profile = new
                        {
                            _id = _accountId,
                            created = DateTime.Now.ToDateTimeString(),
                            updated = DateTime.Now.ToDateTimeString(),
                            rvn = 1,
                            wipeNumber = 1,
                            accountId = _accountId,
                            profileId = "athena",
                            version = "pixelnite_dev_may_2020",
                            items = itemsFormatted,
                            stats = new {
                                attributes = new
                                {
                                    past_seasons = new List<object>(),
                                    season_match_boost = 0,
                                    loadouts = new List<string>
                                    {
                                        "CosmeticLocker:cosmeticlocker_athena1"
                                    },
                                    mfa_reward_claimed = true,
                                    rested_xp_overflow = 0,
                                    quest_manager = new
                                    {
                                        dailyLoginInternal = DateTime.Now.ToDateTimeString(),
                                        dailyQuestRerolls = 1
                                    },
                                    book_level = _account.PassLevel,
                                    season_num = ApiConfig.Current.Season,
                                    season_update = 0,
                                    book_xp = _account.PassXP,
                                    permissions = new List<object>(),
                                    season = new
                                    {
                                        numWins = 999,
                                        numHighBracket = 999,
                                        numLowBracket = 999
                                    },
                                    vote_data = new {},
                                    lifetime_wins = 0,
                                    book_purchased = _account.BattlePass,
                                    purchased_battle_pass_tier_offers = new {},
                                    rested_xp_exchange = 1,
                                    level = _account.Level,
                                    xp_overflow = 0,
                                    rested_xp = 0,
                                    rested_xp_mult = 1,
                                    accountLevel = _account.TotalLevel,
                                    competitive_identity = new {},
                                    inventory_limit_bonus = 0,
                                    last_applied_loadout = "CosmeticLocker:cosmeticlocker_athena1",
                                    daily_rewards = new { },
                                    xp = _account.XP,
                                    season_friend_match_boost = 0,
                                    active_loadout_index = 0
                                }
                            }
                        }
                    }
                },
                serverTime = DateTime.Now.ToDateTimeString(),
                profileCommandRevision = 1,
                responseVersion = 1
            };
            Log.Information("[ProfileController] Retrieved profile 'athena' {AccountId}{Profile}{Revision}", _accountId, response, _revision);

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            //   Response.Write(System.IO.File.ReadAllText("Assets/profile_athena.json"));
            Response.Write(JsonConvert.SerializeObject(response));
        }

        [Route("POST", "/fortnite/api/game/v2/profile/*/client/SetMtxPlatform")]
        public void SetMTXPlatform()
        {
            _accountId = Request.Url.Segments[Request.Url.Segments.Length - 3].Replace("/", "");

            if (!AccountManager.AccountExists(_accountId))
            {
                Response.StatusCode = 404;
                return;
            }
            _account = AccountManager.GetAccount(_accountId);
            Query.TryGetValue("profileId", out string profileId);
            Query.TryGetValue("rvn", out string rvn);
            _revision = Convert.ToInt32(rvn ?? "-2");

            var response = new
            {
                profileChanges = new
                {
                    changeType = "statModified",
                    name = "current_mtx_platform",
                    value = "EpicPC"
                },
                profileChangesBaseRevision = 1,
                profileCommandRevision = 11,
                profileId = "common_core",
                profileRevision = 11,
                responseVersion = 1,
                serverTime = DateTime.Now.ToDateTimeString()
            };
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

    }
}
