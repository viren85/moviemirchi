using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using LuceneSearchLibrarby;
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
        public ActionResult AddMovie(string data)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(data))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                MovieEntity movie = json.Deserialize(data, typeof(MovieEntity)) as MovieEntity;

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

                        //tableMgr.UpdateMovieById(oldEntity);
                    }

                    MovieEntity entity = new MovieEntity();

                    //entity.RowKey = entity.MovieId = Guid.NewGuid().ToString();
                    entity.RowKey = entity.MovieId = movie.MovieId;
                    entity.Name = movie.Name;
                    entity.AltNames = movie.AltNames;
                    entity.Posters = movie.Posters;
                    entity.Ratings = movie.Ratings;
                    entity.Synopsis = movie.Synopsis;
                    entity.Casts = movie.Casts;
                    entity.Stats = movie.Stats;
                    entity.Songs = movie.Songs;
                    entity.Trailers = movie.Trailers;
                    entity.Pictures = movie.Pictures;
                    entity.Genre = movie.Genre;
                    entity.Month = movie.Month;
                    entity.Year = movie.Year;
                    entity.UniqueName = movie.UniqueName;
                    entity.State = movie.State;
                    entity.MyScore = movie.MyScore;
                    entity.JsonString = movie.JsonString;
                    entity.Popularity = movie.Popularity;

                    tableMgr.UpdateMovieById(entity);

                    //delete Entry in lucene search index
                    LuceneSearch.ClearLuceneIndexRecord(movie.MovieId);

                    string posterUrl = "default-movie.jpg";
                    string critics = string.Empty;

                    if (!string.IsNullOrEmpty(entity.Posters))
                    {
                        List<string> pList = json.Deserialize(entity.Posters, typeof(List<string>)) as List<string>;
                        if (pList != null && pList.Count > 0)
                            posterUrl = pList[pList.Count - 1];
                    }

                    var reviewDic = tableMgr.GetReviewsByMovieId(entity.MovieId);

                    if (reviewDic != null && reviewDic.Values != null && reviewDic.Values.Count > 0)
                    {
                        List<string> lCritics = new List<string>();

                        foreach (ReviewEntity re in reviewDic.Values)
                        {
                            lCritics.Add(re.ReviewerName);
                        }

                        critics = json.Serialize(lCritics);
                    }

                    // add updated entry in lucene search index
                    MovieSearchData movieSearchIndex = new MovieSearchData();
                    movieSearchIndex.Id = entity.RowKey;
                    movieSearchIndex.Title = entity.Name;
                    movieSearchIndex.Type = entity.Genre;
                    movieSearchIndex.TitleImageURL = posterUrl;
                    movieSearchIndex.UniqueName = entity.UniqueName;
                    movieSearchIndex.Description = movie.Casts;
                    movieSearchIndex.Critics = critics;
                    movieSearchIndex.Link = entity.UniqueName;
                    LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Movie
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
            catch (Exception)
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
