using ModniteServer.API.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ModniteServer.API.Controllers
{
    internal sealed class StorefrontController : Controller
    {
        /// <summary>
        /// Gets the keys needed to decrypt some assets.
        /// </summary>
        [Route("GET", "/fortnite/api/storefront/v2/keychain")]
        public void GetKeychain()
        {
            // This API gives the encryption key needed to decrypt data in pakchunk1000+


            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(System.IO.File.ReadAllText("Assets/keychain.json"));
        }

        [Route("POST", "/fortnite/api/game/v2/profile/*/client/PurchaseCatalogEntry")]
        public void PurchaseItem()
        {
            string accountId = Request.Url.Segments[Request.Url.Segments.Length - 3].Trim('/');
            Query.TryGetValue("profileId", out string profileId);
            Query.TryGetValue("rvn", out string rvn);

            var response = new
            {
                // TODO
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Since we're never going to sell anything, there's not much to do here.
        /// </summary>
        [Route("GET", "/fortnite/api/receipts/v1/account/*/receipts")]
        public void GetReceipts()
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write("[]");
        }

        [Route("GET", "/fortnite/api/storefront/v2/catalog", isHidden: true)]
        public void GetCatalog()
        {
            var storefronts = new List<object>();
            foreach (Storefront storefront in StorefrontManager.Storefronts)
            {
                var catalogEntries = new List<object>();
                foreach (StoreItem item in storefront.Catalog)
                {
                    var catalogItem = new
                    {
                        devName = item.TemplateId,
                        offerId = "v2:/" + item.Guid,
                        fulfillmentIds = new List<object>(),
                        dailyLimit = -1,
                        weeklyLimit = -1,
                        monthlyLimit = -1,
                        categories = item.Categories,
                        prices = new[]
                        {
                            new
                            {
                                currencyType = "MtxCurrency",
                                currencySubType = "",
                                regularPrice = 0,
                                finalPrice = 0,
                                saleExpiration = DateTime.MaxValue.ToDateTimeString(),
                                basePrice = 0
                            }
                        },
                        matchFilter = "",
                        filterWeight = 0.0,
                        appStoreId = new List<object>(),
                        requirements = new List<object>()
                        {
                            new
                            {
                                requirementType = "DenyOnItemOwnership",
                                requiredId = item.TemplateId,
                                minQuantity = 1
                            }
                        },
                        offerType = "StaticPrice",
                        giftInfo = new
                        {
                            bIsEnabled = false,
                            forcedGiftBoxTemplateId = "",
                            purchaseRequirements = new List<object>(),
                            giftRecordIds = new List<object>()
                        },
                        refundable = true,
                        metaInfo = new List<object>(),
                        displayAssetPath = item.DisplayAssetPath,
                        itemGrants = new []
                        {
                            new
                            {
                                templateId = item.TemplateId,
                                quantity = 1
                            }
                        },
                        sortPriority = item.Priority,
                        catalogGroupPriority = 0
                    };

                    if (!item.DenyOnOwnership)
                    {
                        catalogItem.requirements.Clear();
                    }

                    if (item.BannerType != StoreItemBannerType.None)
                    {
                        if (storefront.IsWeeklyStore)
                        {
                            if (item.BannerType == StoreItemBannerType.New)
                            {
                                catalogItem.metaInfo.Add(new
                                {
                                    key = "StoreToastHeader",
                                    value = "New",
                                });
                                catalogItem.metaInfo.Add(new
                                {
                                    key = "StoreToastBody",
                                    value = "NewSet",
                                });
                            }

                            // TODO: add support for other banners in the weekly store section
                        }
                        else
                        {
                            catalogItem.metaInfo.Add(new
                            {
                                key = "BannerOverride",
                                value = item.BannerType.ToString()
                            });
                        }
                    }

                    catalogEntries.Add(catalogItem);
                }

                storefronts.Add(new
                {
                    name = storefront.Name,
                    catalogEntries
                });
            }

            var response = new
            {
                refreshIntervalHrs = StorefrontManager.RefreshInterval,
                dailyPurchaseHrs = StorefrontManager.RefreshInterval,
                expiration = StorefrontManager.Expiration.ToDateTimeString(),
                storefronts
            };

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));
        }
    }
}