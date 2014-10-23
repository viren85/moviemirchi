
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
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
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/ReviewerInfo?name={id}
        protected override string ProcessRequest()
        {
            string name = "";

            try
            {
                var tableMgr = new TableManager();

                Reviewer reviewerInfo = new Reviewer();
                List<ReviewDetails> reviewDetailList = new List<ReviewDetails>();

                //ReviewerMoviesEntity
                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["name"]))
                {
                    throw new ArgumentException(Constants.API_EXC_REVIEWERNAME_NOT_EXIST);
                }

                name = qpParams["name"].ToString();

                if (!DataStoreLib.Utils.CacheManager.TryGet<Reviewer>(CacheConstants.ReviewerMoviesEntity + name.Replace(" ", "-").ToLower().Trim(), out reviewerInfo))
                {
                    reviewerInfo = new Reviewer();
                    reviewDetailList = new List<ReviewDetails>();

                    // getting reviewer details
                    var reviews = tableMgr.GetReviewsByReviewer(name);

                    if (reviews != null && reviews.Count > 0)
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

                            if (movie != null && (movie.State == "upcoming" || movie.State == "now-playing" || movie.State == "released" || movie.State == "now playing" || movie.State == ""))
                            {
                                // if movie not null, then add movieid and moviename to review details
                                ReviewDetails reviewDetail = new ReviewDetails();
                                reviewDetail.CriticsRating = review.SystemRating == 0 ? "" : (review.SystemRating == -1 ? 0 : 100).ToString();
                                reviewDetail.MovieId = movie.MovieId;
                                reviewDetail.MovieName = movie.Name;
                                reviewDetail.Review = review.Review;
                                reviewDetail.MoviePoster = movie.Posters;
                                reviewDetail.OutLink = review.OutLink;
                                reviewDetail.Tags = review.Tags;
                                reviewDetail.MovieStatus = movie.State;

                                // add review object to review list
                                reviewDetailList.Add(reviewDetail);
                            }

                            courter++;
                        }

                        // add reviewList to reviewInfoObject
                        reviewerInfo.ReviewsDetails = reviewDetailList;

                        DataStoreLib.Utils.CacheManager.Add<Reviewer>(CacheConstants.ReviewerMoviesEntity + name.Replace(" ", "-").ToLower().Trim(), reviewerInfo);
                    }
                    else
                    {
                        return jsonSerializer.Value.Serialize(
                      new
                      {
                          Status = "Error",
                          UserMessage = "Currently Movie Mirchi does not have any information about this critic (" + name + ")",
                          ActualError = string.Empty
                      });
                    }
                }

                // serialize and return reviewer details along with reviews
                return jsonSerializer.Value.Serialize(reviewerInfo);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(
                  new
                  {
                      Status = "Error",
                      UserMessage = "Unable to get reviewer " + name + "'s details.",
                      ActualError = ex.Message
                  });
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
        public string Tags { get; set; }
        public string MovieStatus { get; set; }
    }
}
