namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Web;
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
                    int rating;

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]) && !string.IsNullOrEmpty(qpParams["rating"]))
                    {
                        movieId = qpParams["movieid"].ToString();
                        reviewId = qpParams["reviewid"].ToString();
                        bool isBOW = !string.IsNullOrEmpty(qpParams["bagofwords"]);
                        bagOfWord = isBOW ? qpParams["bagofwords"].ToString() : string.Empty;

                        if (int.TryParse(qpParams["rating"].ToString(), out rating))
                        {
                            MovieEntity movie = tableMgr.GetMovieById(movieId);

                            if (movie != null)
                            {
                                ReviewEntity review = tableMgr.GetReviewById(reviewId);

                                if (review != null)
                                {
                                    // -1 => Negative
                                    //  0 => No rating
                                    // +1 => Positive
                                    rating = (rating < 0) ? -1 : 1;

                                    review.SystemRating = rating;
                                    tableMgr.UpdateReviewById(review);

                                    string myscore = movie.MyScore;
                                    if (string.IsNullOrEmpty(myscore) || myscore == "0")
                                    {
                                        myscore = "{\"teekharating\":\"0\",\"feekharating\":\"0\",\"criticrating\":\"\"}";
                                    }

                                    RatingConvertion newRating = new RatingConvertion();
                                    RatingConvertion oldRating;
                                    try
                                    {
                                        oldRating = jsonSerializer.Value.Deserialize(myscore, typeof(RatingConvertion)) as RatingConvertion;
                                    }
                                    catch
                                    {
                                        myscore = "{\"teekharating\":\"0\",\"feekharating\":\"0\",\"criticrating\":\"\"}";
                                        oldRating = jsonSerializer.Value.Deserialize(myscore, typeof(RatingConvertion)) as RatingConvertion;
                                    }

                                    var teekha = (oldRating.teekharating + rating);
                                    var feekha = (oldRating.feekharating + (1 - rating));
                                    newRating.teekharating = teekha;
                                    newRating.feekharating = feekha;
                                    newRating.criticrating = ((int)(teekha / (double)(teekha + feekha) * 100)).ToString();

                                    string strNewRating = jsonSerializer.Value.Serialize(newRating);
                                    movie.Ratings = newRating.criticrating;
                                    movie.MyScore = strNewRating;
                                    tableMgr.UpdateMovieById(movie);

                                    return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMassege = "Successfully update movie rating" });
                                }
                                else
                                {
                                    return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to find review with passed review id. Please check review id." });
                                }
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

            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Error occured while updating movie rating", ActualError = "Some of parameter is empty" });
        }
    }

    public class RatingConvertion
    {
        public int teekharating { get; set; }
        public int feekharating { get; set; }
        public string criticrating { get; set; }
    }
}
