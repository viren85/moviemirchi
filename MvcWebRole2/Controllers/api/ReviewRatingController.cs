namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Storage;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This api take movieId,reviewId,review text and call exe file which process the reviews and calculate it rating    
    /// </summary>
    public class ReviewRatingController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/ReviewRating?movieid=<mid>&reviewid=<rid>&rating=<rating>          
        protected override string ProcessRequest()
        {
            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;

            try
            {
                if (queryParameters != null)
                {
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]) && !string.IsNullOrEmpty(qpParams["rating"]))
                    {
                        string movieId = qpParams["movieid"].ToString();
                        string reviewId = qpParams["reviewid"].ToString();
                        string rating = qpParams["rating"].ToString();

                        var tableMgr = new TableManager();

                        bool result = tableMgr.UpdateReviewRating(reviewId, rating);
                        if (!result)
                        {
                            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Could not save the review rating", ActualError = "Unknown" });
                        }
                        else
                        {
                            // Update the movie rating
                        }

                        return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMessage = "Successfully saved the rating" });
                    }
                }

            }
            catch (Exception ex)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Unable to save the review rating", ActualError = ex.Message });
            }

            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Unable to save the review rating", ActualError = "Some of the parameters is empty" });
        }
    }
}
