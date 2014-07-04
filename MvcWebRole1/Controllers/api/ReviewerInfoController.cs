
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns all the information about the reviewer. This includes all the reviews written by this reviewer.
    /// throws ArgumentException
    /// </summary>
    public class ReviewerInfoController : BaseController
    {
        // get : api/ReviewerInfo?name={id}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();

                Reviewer reviewerInfo = new Reviewer();
                List<ReviewDetails> reviewDetailList = new List<ReviewDetails>();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["name"]))
                {
                    throw new ArgumentException(Constants.API_EXC_REVIEWERNAME_NOT_EXIST);
                }

                string name = qpParams["name"].ToString();

                // getting reviewer details
                var reviews = tableMgr.GetReviewsByReviewer(name);

                if (reviews != null)
                {
                    int courter = 0;

                    foreach (ReviewEntity review in reviews.Values)
                    {
                        if (courter == 0)
                        {
                            // getting reviewer Informations
                            reviewerInfo.Affilation = review.Affiliation;
                            reviewerInfo.Name = review.ReviewerName;
                            reviewerInfo.OutLink = review.OutLink;
                        }
                        else if (review.MovieId == null)
                            continue;

                        // get movie information
                        MovieEntity movie = tableMgr.GetMovieById(review.MovieId);

                        if (movie != null && (movie.State == "upcoming" || movie.State == "now-playing" || movie.State == ""))
                        {
                            // if movie not null, then add movieid and moviename to review details
                            ReviewDetails reviewDetail = new ReviewDetails();
                            reviewDetail.CriticsRating = review.ReviewerRating;
                            reviewDetail.MovieId = movie.MovieId;
                            reviewDetail.MovieName = movie.Name;
                            reviewDetail.Review = review.Review;
                            reviewDetail.MoviePoster = movie.Posters;
                            reviewDetail.OutLink = review.OutLink;
                            reviewDetail.MovieStatus = movie.State;

                            // add review object to review list
                            reviewDetailList.Add(reviewDetail);
                        }

                        courter++;
                    }
                }

                // add reviewList to reviewInfoObject
                reviewerInfo.ReviewsDetails = reviewDetailList;

                // serialize and return reviewer details along with reviews
                return json.Serialize(reviewerInfo);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_REVIEWER_INFO, ActualError = ex.Message });
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
        public string CriticsRating { get; set; }
        public string Review { get; set; }
        public string ReviewDate { get; set; }
        public string OutLink { get; set; }
        public string MoviePoster { get; set; }

        public string MovieStatus { get; set; }
    }
}
