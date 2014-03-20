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

namespace MvcWebRole2.Controllers
{
    public class ReviewController : Controller
    {
        #region Set Connection string
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        [HttpGet]
        public ActionResult AddMovieReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMovieReview(string reviewJson)
        {
            if (string.IsNullOrEmpty(reviewJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();


                ReviewEntity affil = json.Deserialize(reviewJson, typeof(ReviewEntity)) as ReviewEntity;
                if (affil != null)
                {
                    SetConnectionString();
                    ReviewEntity entity = new ReviewEntity();
                    TableManager tblMgr = new TableManager();
                    //var review = tblMgr.GetReviewDetailById(entity.ReviewerId, entity.MovieId);

                    if (affil.ReviewId != "")
                    {
                        return Json(new { Status = "Exist" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        entity.RowKey = entity.ReviewId = Guid.NewGuid().ToString();
                        entity.MovieId = affil.MovieId;
                        entity.ReviewerId = affil.ReviewerId;
                        entity.ReviewerRating = affil.ReviewerRating;
                        entity.Review = affil.Review;
                        entity.OutLink = affil.OutLink;
                        entity.Summary = affil.Summary;
                        tblMgr.UpdateReviewById(entity);
                    }
                    

                    //TableManager tblMgr = new TableManager();
                   
                }
                else
                {
                    return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateMovieReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateMovieReview(string hfUpdateMovieReview)
        {
            SetConnectionString();

            if (string.IsNullOrEmpty(hfUpdateMovieReview))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReviewEntity review = json.Deserialize(hfUpdateMovieReview, typeof(ReviewEntity)) as ReviewEntity;

                if (review != null)
                {
                    TableManager tblMgr = new TableManager();
                    ReviewEntity entity = new ReviewEntity();

                    entity.RowKey = entity.ReviewId = review.ReviewId;
                    entity.MovieId = review.MovieId;
                    entity.ReviewerId = review.ReviewerId;
                    entity.ReviewerRating = review.ReviewerRating;
                    entity.Review = review.Review;
                    entity.OutLink = review.OutLink;
                    entity.Summary = review.Summary;

                    tblMgr.UpdateReviewById(entity);
                }
            }
            catch (Exception)
            {

                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }



            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReviewDetailByReviewerId(string query)
        {
            string[] ids = query.Split(',');
            TableManager tblMgr = new TableManager();
            var review = tblMgr.GetReviewDetailById(ids[0], ids[1]);
            return Json(review, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetReviewer()
        {
            var tableMgr = new TableManager();
            var reviewers = tableMgr.GetSortedReviewerByName().Select(
                x => new SelectListItem
                {
                    Value = x.ReviewerId.ToString(),
                    Text = x.ReviewerName
                }
                );
            return new SelectList(reviewers, "Value", "Text");
        }

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



    }
}
