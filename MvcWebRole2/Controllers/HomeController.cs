using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        #region Set connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Artists()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateArtists(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ArtistEntity movie = json.Deserialize(data, typeof(ArtistEntity)) as ArtistEntity;

                if (movie != null)
                {
                    string uniqueName = movie.ArtistName.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                    var tableMgr = new TableManager();

                    ArtistEntity entity = new ArtistEntity();

                    entity.RowKey = entity.ArtistId = movie.ArtistId;
                    entity.ArtistName = movie.ArtistName;
                    entity.Bio = movie.Bio;
                    entity.Posters = movie.Posters;
                    entity.Born = movie.Born;
                    entity.MovieList = movie.MovieList;                    
                    entity.UniqueName = movie.UniqueName;                    
                    entity.MyScore = movie.MyScore;
                    entity.JsonString = movie.JsonString;
                    entity.Popularity = movie.Popularity;

                    tableMgr.UpdateArtistById(entity);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
    }
}
