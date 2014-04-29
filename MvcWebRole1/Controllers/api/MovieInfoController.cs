
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
    /// This API returns single movie object based on the movie name.
    /// If movie result found, return JSON string contains, all the movie information including reviews. Otherwise, return empty string.
    /// throw ArgumentException
    /// </summary>
    public class MovieInfoController : BaseController
    {
        // get : api/MovieInfo?q={movieId}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            
            try
            {
                var tableMgr = new TableManager();
                MovieInfo movieInfo = new MovieInfo();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["q"]))
                {
                    throw new ArgumentException(Constants.API_EXC_MOVIE_NAME_NOT_EXIST);
                }

                string name = qpParams["q"].ToString();

                // get single movie object form database by its uinque name
                var movie = tableMgr.GetMovieByUniqueName(name);

                if (movie != null)
                {
                    List<ReviewEntity> userReviews = new List<ReviewEntity>();

                    movieInfo.movieId = movie.MovieId;
                    movieInfo.Movie = movie;

                    
                    // get reviews for movie by movie id
                    var reviewList = tableMgr.GetReviewsByMovieId(movie.MovieId);

                    if (reviewList != null)
                    {
                        // if reviews not null then add review to review list.
                        foreach (var review in reviewList)
                        {
                            ReviewerEntity reviewer = tableMgr.GetReviewerById(review.Value.ReviewerId);
                            ReviewEntity objReview = review.Value as ReviewEntity;

                            objReview.ReviewerName = reviewer.ReviewerName;
                            objReview.OutLink = reviewer.ReviewerImage;
                            userReviews.Add(objReview);
                        }
                    }

                    //add reviewlist to movieinfo reviews
                    movieInfo.MovieReviews = userReviews;
                    
                    // serialize movie object and return.
                    return json.Serialize(movieInfo);
                }
                else
                {
                    // if movie not found then return empty string
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_MOVIE, ActualError = ex.Message });
            }
        }
    }
}
