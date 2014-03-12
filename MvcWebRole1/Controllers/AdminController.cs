using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

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
                //return View();
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
                    //entity.UniqueName = movie.Name;
                    entity.Synopsis = movie.Synopsis;
                    entity.Posters = movie.Posters;
                    entity.Genre = movie.Genre;
                    entity.Month = movie.Month;
                    entity.Year = movie.Year;
                    entity.AltNames = movie.Name;

                    string uniqueName = movie.Name.Replace(" ", "_").Replace("&", "_and_").Replace(".", "").Replace("'", "").ToLower();
                    entity.UniqueName = uniqueName;

                    tableMgr.UpdateMovieById(entity);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
                //throw new ArgumentException("Error Found in Admin controller");
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
            //return View();
        }


        [HttpGet]
        public ActionResult MovieReview()
        {
            SetConnectionString();

            ViewBag.movie = GetMovie();


            return View();
        }


        /// <summary>
        /// Passing json object as param
        /// </summary>
        /// <param name="hfReview"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MovieReview(string hfReview)
        {
            if (string.IsNullOrEmpty(hfReview))
            {
                return View();
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReviewEntity review = json.Deserialize(hfReview, typeof(ReviewEntity)) as ReviewEntity;

                if (review != null)
                {
                    SetConnectionString();
                    ReviewEntity entity = new ReviewEntity();
                    var rand = new Random((int)DateTimeOffset.UtcNow.Ticks);

                    //var m = new MovieEntity();
                    //entity.MovieId = m.MovieId = Guid.NewGuid().ToString();
                    //entity.ReviewId = 
                    entity.MovieId = review.MovieId;
                    entity.RowKey = entity.ReviewId = Guid.NewGuid().ToString();
                    entity.Review = review.Review;
                    entity.ReviewerName = review.ReviewerName;
                    entity.ReviewerRating = review.ReviewerRating;
                    entity.SystemRating = review.SystemRating;
                    entity.OutLink = review.OutLink;
                    entity.Affiliation = review.Affiliation;
                    entity.Hot = review.Hot;

                    var tableMgr = new TableManager();
                    tableMgr.UpdateReviewById(entity);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewBag.movie = GetMovie();

            return View();
        }


        //public IEnumerable<SelectListItem> GetMovie()
        //{
        //    var tableMgr = new TableManager();
        //    var movies = tableMgr.GetSortedMoviesByName().Select(
        //        x => new SelectListItem
        //        {
        //            Value = x.MovieId.ToString(),
        //            Text = x.Name
        //        }
        //        );
        //    return new SelectList(movies, "Value", "Text");  selectlist
        //}

        public IEnumerable<SelectListItem> GetMovie()
        {
            var tableMgr = new TableManager();
            var movies = tableMgr.GetSortedMoviesByName().Select(
                x => new SelectListItem
                {
                    Value = x.MovieId.ToString(),
                    Text = x.Name
                }
                );
            return new SelectList(movies, "Value", "Text");
        }


        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }


    }
}
