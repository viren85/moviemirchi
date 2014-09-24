
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns single movie object based on the movie name.
    /// If movie result found, return JSON string contains, all the movie information including reviews. Otherwise, return empty string.
    /// throw ArgumentException
    /// </summary>
    public class MovieInfoController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/MovieInfo?q={movieId}
        protected override string ProcessRequest()
        {
            // get query string parameters
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrEmpty(qpParams["q"]))
            {
                throw new ArgumentException(Constants.API_EXC_MOVIE_NAME_NOT_EXIST);
            }

            string name = qpParams["q"].ToString().ToLower();

            string json;
            if (!CacheManager.TryGet<string>(CacheConstants.MovieInfoJson + name, out json))
            {

                try
                {
                    var tableMgr = new TableManager();

                    // get single movie object form database by its unique name
                    var movie = tableMgr.GetMovieByUniqueName(name);

                    if (movie != null)
                    {
                        MovieInfo movieInfo = new MovieInfo();
                        movieInfo.movieId = movie.MovieId;
                        movieInfo.Movie = movie;

                        // get reviews for movie by movie id
                        var reviewList = tableMgr.GetReviewsByMovieId(movie.MovieId);

                        // if reviews not null then add review to review list.
                        var userReviews = (reviewList != null) ?
                            reviewList.Select(review =>
                            {
                                ReviewerEntity reviewer = tableMgr.GetReviewerById(review.Value.ReviewerId);
                                ReviewEntity objReview = review.Value as ReviewEntity;

                                objReview.ReviewerName = reviewer.ReviewerName;
                                objReview.CriticsRating = objReview.SystemRating == 0 ? "" : (objReview.SystemRating == -1 ? 0 : 100).ToString();

                                //objReview.OutLink = reviewer.ReviewerImage;
                                return objReview;
                            }) :
                            Enumerable.Empty<ReviewEntity>();

                        //add reviewlist to movieinfo reviews
                        movieInfo.MovieReviews = userReviews.ToList();

                        // serialize movie object and return.
                        json = jsonSerializer.Value.Serialize(movieInfo);
                    }
                    else
                    {
                        // if movie not found then return empty string
                        json = string.Empty;
                    }

                    CacheManager.Add<string>(CacheConstants.MovieInfoJson + name, json);
                }
                catch (Exception ex)
                {
                    // if any error occured then return User friendly message with system error message
                    // use jsonError here because more custumizable
                    return jsonSerializer.Value.Serialize(
                    new
                    {
                        Status = "Error",
                        UserMessage = "Unable to find " + name + " movie.",
                        ActualError = ex.Message
                    });
                }
            }

            return json;
        }
    }
}
