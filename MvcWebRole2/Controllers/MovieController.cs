using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole2.Controllers
{
    public class MovieController : Controller
    {
        #region Set connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        #region Add Moive
        [HttpGet]
        public ActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMovie(string hfMovie)
        {
            if (string.IsNullOrEmpty(hfMovie))
            {                
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                MovieEntity movie = json.Deserialize(hfMovie, typeof(MovieEntity)) as MovieEntity;

                if (movie != null)
                {
                    SetConnectionString();

                    var tableMgr = new TableManager();
                    MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(movie.Name);

                    if (oldEntity != null)
                    {
                        var rand = new Random();
                        int num = rand.Next(999999);

                        oldEntity.UniqueName = num.ToString() + oldEntity.UniqueName;

                        tableMgr.UpdateMovieById(oldEntity);
                    }

                    MovieEntity entity = new MovieEntity();

                    entity.RowKey = entity.MovieId = Guid.NewGuid().ToString();
                    entity.Stats = movie.Stats;
                    entity.Songs = movie.Songs;
                    entity.Ratings = movie.Ratings;
                    entity.Trailers = movie.Trailers;
                    entity.Casts = movie.Casts;
                    entity.Pictures = movie.Pictures;
                    entity.Name = movie.Name;                    
                    entity.Synopsis = movie.Synopsis;
                    entity.Posters = movie.Posters;
                    entity.Genre = movie.Genre;
                    entity.Month = movie.Month;
                    entity.Year = movie.Year;
                    entity.AltNames = movie.Name;

                    string uniqueName = movie.Name.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();
                    entity.UniqueName = uniqueName;

                    tableMgr.UpdateMovieById(entity);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);                
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Update Moive
        [HttpGet]
        public ActionResult UpdateMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateMovie(string movie)
        {
            return View();
        }

        #endregion
    }
}
