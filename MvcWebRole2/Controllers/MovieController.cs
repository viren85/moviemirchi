using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
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
            SetConnectionString();

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
                    string uniqueName = movie.Name.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                    var tableMgr = new TableManager();
                    //MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(movie.Name);
                    MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(uniqueName);

                    

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
                    entity.AltNames = movie.AltNames;
                    entity.UniqueName = uniqueName;
                    
                    tableMgr.UpdateMovieById(entity);

                    List<Cast> casts = json.Deserialize(entity.Casts, typeof(List<Cast>)) as List<Cast>;
                    List<String> actors = new List<string>();

                    if (casts != null)
                    {
                        //List<string> act = new List<string>();
                        foreach (var actor in casts)
                        {
                            actors.Add(actor.name);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);                
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Update Moive
        [HttpGet]
        public ActionResult UpdateMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateMovie(string movieJson)
        {
            if (string.IsNullOrEmpty(movieJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                MovieEntity movie = json.Deserialize(movieJson, typeof(MovieEntity)) as MovieEntity;

                if (movie != null)
                {
                    SetConnectionString();

                    var tableMgr = new TableManager();
                    
                    MovieEntity entity = new MovieEntity();

                    entity.RowKey = entity.MovieId = movie.MovieId;
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
                    entity.AltNames = movie.AltNames;
                    entity.UniqueName = movie.UniqueName;

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

        public ActionResult GetMovieDetailsById(string query)
        {
            var movie = new TableManager().GetMovieById(query); 

            return Json(movie, JsonRequestBehavior.AllowGet);
        }
    }
}
