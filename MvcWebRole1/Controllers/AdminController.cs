
namespace MvcWebRole1.Controllers
{
    using CloudMovie.APIRole.API;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

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

                    string uniqueName = movie.Name.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();
                    entity.UniqueName = uniqueName;

                    tableMgr.UpdateMovieById(entity);
                }

            }
            catch (Exception)
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
                    ReviewEntity entity = new ReviewEntity();
                    var rand = new Random((int)DateTimeOffset.UtcNow.Ticks);

                    //var m = new MovieEntity();
                    //entity.MovieId = m.MovieId = Guid.NewGuid().ToString();
                    //entity.ReviewId = 
                    entity.MovieId = review.MovieId;
                    //entity.RowKey = entity.ReviewId = Guid.NewGuid().ToString();
                    entity.Review = review.Review;
                    entity.ReviewerName = review.ReviewerName;
                    entity.ReviewerRating = review.ReviewerRating;
                    //entity.SystemRating = review.SystemRating;
                    entity.OutLink = review.OutLink;
                    entity.Affiliation = review.Affiliation;
                    entity.Hot = review.Hot;
                    //enitity.

                    var tableMgr = new TableManager();
                    tableMgr.UpdateReviewById(entity);
                }


            }
            catch (Exception)
            {
                throw;
            }

            ViewBag.movie = GetMovie();

            return View();
        }

        public IEnumerable<SelectListItem> GetMovie()
        {
            var tableMgr = new TableManager();
            var movies = tableMgr.GetSortedMoviesByName()
                .Select(
                    x => new SelectListItem
                    {
                        Value = x.MovieId.ToString(),
                        Text = x.Name
                    });
            return new SelectList(movies, "Value", "Text");
        }

        [HttpGet]
        public ActionResult Affilation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Affilation(string hfAffilations)
        {
            if (string.IsNullOrEmpty(hfAffilations))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                AffilationEntity affil = json.Deserialize(hfAffilations, typeof(AffilationEntity)) as AffilationEntity;
                if (affil != null)
                {
                    AffilationEntity entity = new AffilationEntity();

                    entity.RowKey = entity.AffilationId = Guid.NewGuid().ToString();
                    entity.AffilationName = affil.AffilationName;
                    entity.WebsiteName = affil.WebsiteName;
                    entity.WebsiteLink = affil.WebsiteLink;
                    entity.LogoLink = affil.LogoLink;
                    entity.Country = affil.Country;

                    TableManager tblMgr = new TableManager();
                    tblMgr.UpdateAffilationById(entity);
                    // ModelState.Clear();
                }
                else
                {
                    TempData["Error"] = "Please provide required information";
                }


            }
            catch (Exception)
            {

                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            //ModelState.Clear();
            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public ActionResult Reviewer()
        {
            ViewBag.affilation = GetAffilationName();
            return View();
        }

        [HttpPost]
        public ActionResult Reviewer(string hfReviewer)
        {
            if (string.IsNullOrEmpty(hfReviewer))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReviewerEntity reviewer = json.Deserialize(hfReviewer, typeof(ReviewerEntity)) as ReviewerEntity;
                if (reviewer != null)
                {
                    ReviewerEntity entity = new ReviewerEntity();

                    entity.RowKey = entity.ReviewerId = Guid.NewGuid().ToString();
                    entity.ReviewerName = reviewer.ReviewerName;
                    entity.ReviewerImage = reviewer.ReviewerImage;

                    AffilationEntity affil = new AffilationEntity();

                    affil.AffilationId = reviewer.Affilation;


                    //AffilationEntity affil = new AffilationEntity();
                    //if (reviewer.Affilation != null)
                    //{                   
                    //    string affilId = reviewer.Affilation;


                    //    AffilationEntity affil = new AffilationEntity();

                    //    affil = 

                    //    JavaScriptSerializer json1 = new JavaScriptSerializer();
                    //    entity.Affilation = json1.Serialize(affil);
                    //  // affil = (from affilId in ) 




                    //}
                    // entity.Affilation = json1.Serialize(affil);
                    // entity.Affilation = reviewer.Affilation;


                    TableManager tblMgr = new TableManager();
                    var a = tblMgr.GetAffilationById(affil.AffilationId);
                    //JavaScriptSerializer json1 = new JavaScriptSerializer();

                    entity.Affilation = json.Serialize(a);

                    tblMgr.UpdateReviewerById(entity);
                    // ModelState.Clear();
                }
                else
                {
                    TempData["Error"] = "Please provide required information";
                }


            }
            catch (Exception)
            {

                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            //ModelState.Clear();
            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);

        }

        public IEnumerable<SelectListItem> GetAffilationName()
        {
            var tableMgr = new TableManager();
            var affilation = tableMgr.GetSortedAffilationByName().Select(
                x => new SelectListItem
                {
                    Value = x.AffilationId.ToString(),
                    Text = x.AffilationName
                }
                );


            return new SelectList(affilation, "Value", "Text");
        }
    }
}
