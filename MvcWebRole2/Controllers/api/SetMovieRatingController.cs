namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This api take movieId,reviewId,rating as number and bagofword is rating details like teekha,feeka and critic
    /// and Set rating and bagofword with movie whose movieid passed in querystring and return operation status in JSON.
    /// </summary>
    public class SetMovieRatingController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/SetMovieRating?movieid=<mid>&reviewid=<rid>&rating=<num_rating>&bagofwords=<bag>          
        protected override string ProcessRequest()
        {
            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;

            try
            {
                if (queryParameters != null)
                {
                    var tableMgr = new TableManager();
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);

                    string movieId = string.Empty, reviewId = string.Empty, bagOfWord = string.Empty;
                    int rating = -1;

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]) && !string.IsNullOrEmpty(qpParams["rating"]) && !string.IsNullOrEmpty(qpParams["bagofwords"]))
                    {
                        movieId = qpParams["movieid"].ToString();
                        reviewId = qpParams["reviewid"].ToString();
                        bagOfWord = qpParams["bagofwords"].ToString();
                        int.TryParse(qpParams["rating"].ToString(), out rating);

                        if (rating != -1)
                        {
                            MovieEntity movie = tableMgr.GetMovieById(movieId);

                            if (movie != null)
                            {
                                movie.Ratings = rating.ToString();

                                if (string.IsNullOrEmpty(movie.MyScore) || movie.MyScore == "0") movie.MyScore = "{\"teekharating\":\"\",\"feekharating\":\"\",\"criticrating\":\"\"}";

                                RatingConvertion oldRating = jsonSerializer.Value.Deserialize(movie.MyScore, typeof(RatingConvertion)) as RatingConvertion;
                                RatingConvertion queryRating = jsonSerializer.Value.Deserialize(bagOfWord, typeof(RatingConvertion)) as RatingConvertion;
                                RatingConvertion newRating = new RatingConvertion();

                                if (oldRating != null && queryRating != null)
                                {
                                    newRating.teekharating = oldRating.teekharating;
                                    newRating.feekharating = oldRating.feekharating;
                                    newRating.criticrating = oldRating.criticrating;

                                    newRating.very = queryRating.very;
                                    newRating.good = queryRating.good;
                                    newRating.bad = queryRating.bad;
                                }

                                string strNewRating = jsonSerializer.Value.Serialize(newRating);

                                movie.MyScore = strNewRating;

                                tableMgr.UpdateMovieById(movie);

                                return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMassege = "Successfully update movie rating" });
                            }
                            else
                            {
                                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to find movie with passed movie id. Please check movie id." });
                            }
                        }
                    }                    
                }
               
            }
            catch (Exception ex)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Error occured while updating movie rating", ActualError = ex.Message });
            }

            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Error occured while updating movie rating" , ActualError = "Some of parameter is empty"});
        }
    }

    public class RatingConvertion
    {
        public string very { get; set; }
        public string good { get; set; }
        public string bad { get; set; }
        public string teekharating { get; set; }
        public string feekharating { get; set; }
        public string criticrating { get; set; }
    }
}
