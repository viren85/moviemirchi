namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Storage;
    using MvcWebRole2.Controllers.Library;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This api take movieId,reviewId and call exe file which process the reviews and calculate it rating    
    /// </summary>
    public class CalculateRatingController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/CalculateRating?movieid=<mid>&reviewid=<rid>
        protected override string ProcessRequest()
        {
            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;

            try
            {
                if (queryParameters != null)
                {
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]))
                    {
                        string movieId = qpParams["movieid"].ToString();
                        string reviewId = qpParams["reviewid"].ToString();

                        return Scorer.QueueScoreReview(
                            movieId,
                            reviewId);
                    }
                }

            }
            catch (Exception ex)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to launch exe file", ActualError = ex.Message });
            }

            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to launch exe file", ActualError = "Some of parameter is empty" });
        }
    }
}
