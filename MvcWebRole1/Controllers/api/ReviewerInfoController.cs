using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using DataStoreLib.Storage;
using System.Web;
using DataStoreLib.Models;

namespace MvcWebRole1.Controllers.api
{
    public class ReviewerInfoController : BaseController
    {
        // get : api/ReviewerInfo?name={id}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            Reviewer reviewerInfo = new Reviewer();

            List<ReviewDetails> reviewDetailList = new List<ReviewDetails>();

            try
            {
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["name"]))
                {
                    throw new ArgumentException("name is not present");
                }

                string name = qpParams["name"].ToString();

                var tableMgr = new TableManager();
                var reviews = tableMgr.GetReviewsByReviewer(name);

                if (reviews != null)
                {
                    int courter = 0;

                    foreach (ReviewEntity review in reviews.Values)
                    {
                        if (courter == 0)
                        {
                            reviewerInfo.Affilation = review.Affiliation;
                            reviewerInfo.Name = review.ReviewerName;
                            reviewerInfo.OutLink = review.OutLink;
                        }

                        MovieEntity movie = tableMgr.GetMovieById(review.MovieId);

                        if (movie != null)
                        {
                            ReviewDetails reviewDetail = new ReviewDetails();
                            reviewDetail.CriticsRating = review.ReviewerRating;
                            reviewDetail.MovieId = movie.MovieId;
                            reviewDetail.MovieName = movie.Name;
                            reviewDetail.Review = review.Review;

                            reviewDetailList.Add(reviewDetail);
                        }

                        courter++;
                    }
                }

                reviewerInfo.ReviewsDetails = reviewDetailList;

                return json.Serialize(reviewerInfo);

            }
            catch (Exception ex)
            {
                return json.Serialize(new { Error = "There are some error!", ActualError = ex.Message });
            }
        }
    }

    public class Reviewer
    {
        public string Name { get; set; }
        public string OutLink { get; set; }
        public string Affilation { get; set; }
        public List<ReviewDetails> ReviewsDetails { get; set; }
    }

    public class ReviewDetails
    {
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public int CriticsRating { get; set; }
        public string Review { get; set; }
        public string ReviewDate { get; set; }
    }
}
