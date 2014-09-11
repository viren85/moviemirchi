namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using MvcWebRole2.Controllers.Library;
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
                            return Scorer.SetReviewAndUpdateMovieRating(
                                movieId,
                                reviewId,
                                rating,
                                bagOfWord);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Error occured while updating movie rating", ActualError = ex.Message });
            }

            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Error occured while updating movie rating", ActualError = "Some of parameter is empty" });
        }
    }

    public class RatingConvertion
    {
        public int teekharating { get; set; }
        public int feekharating { get; set; }
        public string criticrating { get; set; }
    }
}
