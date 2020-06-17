using Serilog;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ModniteServer.API.Controllers
{
    public class CloudStorage
    {
       public string uniqueFilename { get; set; }
       public string filename { get; set; }
       public string hash { get; set; }
       public string hash256 { get; set; }
       public int length { get; set; }
       public string contentType { get; set; }
       public string uploaded { get; set; }
       public string storageType { get; set; }
       public bool doNotCache { get; set; }
    }
    internal sealed class CloudStorageController : Controller
    {
        /// <summary>
        /// Gets the list of system files to download from the cloud storage service.
        /// </summary>
        [Route("GET", "/fortnite/api/cloudstorage/system")]
        public void GetSystemFileList()
        {
            if (!Authorize()) { }

            IList<CloudStorage> DefaultEngine = new List<CloudStorage>
            {
                new CloudStorage
                {
                    uniqueFilename = "3460cbe1c57d4a838ace32951a4d7171",
                    filename = "DefaultEngine.ini",
                    hash = "b59ad50dee2a1a5e0baae8c2dc7aa0006e9b72e8",
                    hash256 = "f46ee3cb34e9dcdbbc8df59600066557a5d01f3a94066d42df9903d639603578",
                    length = 141,
                    contentType = "application/octet-stream",
                    uploaded =  DateTime.Now.AddDays(-2).ToDateTimeString(),
                    storageType = "S3",
                    doNotCache = false
                }
            };

            IList<CloudStorage> DefaultGame = new List<CloudStorage>
            {
                new CloudStorage
                {
                    uniqueFilename = "a22d837b6a2b46349421259c0a5411bf",
                    filename = "DefaultGame.ini",
                    hash = "633f7326836f4c5bc43f07eea63dfb969199083e",
                    hash256 = "1b515f08221fceceb917fec31c0aab8dd58df9a4b42a9f03685b857cf2249902",
                    length = 45,
                    contentType = "application/octet-stream",
                    uploaded =  DateTime.Now.AddDays(-2).ToDateTimeString(),
                    storageType = "S3",
                    doNotCache = false
                }
            };
            JArray response = new JArray(

                DefaultEngine.Select(p => new JObject 
                {
                    {"uniqueFilename", p.uniqueFilename },
                    {"filename", p.filename },
                    {"hash", p.hash },
                    {"hash256", p.hash256 },
                    {"length", p.length },
                    {"uploaded", p.uploaded },
                    {"storageType", p.storageType },
                    {"doNotCache", p.doNotCache }
                }),
                DefaultGame.Select(p => new JObject
                {
                    {"uniqueFilename", p.uniqueFilename },
                    {"filename", p.filename },
                    {"hash", p.hash },
                    {"hash256", p.hash256 },
                    {"length", p.length },
                    {"uploaded", p.uploaded },
                    {"storageType", p.storageType },
                    {"doNotCache", p.doNotCache }
                })
                );
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(response));

            /* 
             * [
             *   {
             *     "uniqueFileName": "<some hashed filename>",
             *     "filename": "DefaultRuntimeOptions.ini",
             *     "hash": "<hash>"
             *     "hash256": "<hash>"
             *     "length": <bytes>
             *     "contentType": "application/octet-stream"
             *     "uploaded": "<ISO date>"
             *     "storageType": "S3"
             *     "doNotCache": bool
             * ]
             */
        }
        [Route("GET", "/fortnite/api/cloudstorage/system/3460cbe1c57d4a838ace32951a4d7171")]
        public void GetDefaultEngine()
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/octet-stream";
            Response.Write(File.ReadAllText("Assets/cloudstorage/DefaultEngine.ini"));
            Log.Information($"[CLOUDSTORAGE] Fortnite accessed DefaultEngine.ini");
        }

        [Route("GET", "/fortnite/api/cloudstorage/system/a22d837b6a2b46349421259c0a5411bf")]
        public void GetDefaultGame()
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/octet-stream";
            Response.Write(File.ReadAllText("Assets/cloudstorage/DefaultGame.ini"));
            Log.Information($"[CLOUDSTORAGE] Fortnite accessed DefaultGame.ini");
        }

        [Route("GET", "/fortnite/api/cloudstorage/user/*")]
        public void GetUserFileList()
        {
            // TODO: Implement cloud storage system
            if (!Authorize()) { }

            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            Response.Write("[]");
        }

        [Route("GET", "/fortnite/api/cloudstorage/user/*/*")]
        public void GetUserFile()
        {
            if (!Authorize()) { }

            string accountId = Request.Url.Segments.Reverse().Skip(1).Take(1).Single();
            string file = Request.Url.Segments.Last();

            if (file.ToLowerInvariant() == ">.sav")
            {
                // For now, we're giving the default settings. Eventually, we'll be storing user settings.
                Response.StatusCode = 204;
              //  Response.ContentType = "application/octet-stream";
              //  Response.Write(File.ReadAllBytes("Assets/ClientSettings.Sav"));
            }
            else
            {
                Response.StatusCode = 404;
            }

            Log.Information($"{accountId} downloaded {file} {{AccountId}}{{File}}", accountId, file);
        }

        // todo
        [Route("PUT", "/fortnite/api/cloudstorage/user/*/*")]
        public void SaveUserFile()
        {
            if (!Authorize()) { }

            string accountId = Request.Url.Segments.Reverse().Skip(1).Take(1).Single();
            string file = Request.Url.Segments.Last();

            Response.StatusCode = 204;
        }
    }
}