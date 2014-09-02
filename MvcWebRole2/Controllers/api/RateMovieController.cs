namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class RateMovieController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/RateMovie?movieid=<mid>
        protected override string ProcessRequest()
        {
            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                string movieId = qpParams["movieid"];

                if (!string.IsNullOrEmpty(movieId))
                {
                    var tableMgr = new TableManager();
                    var movie = tableMgr.GetMovieById(movieId);
                    if (movie != null)
                    {
                        movie.MyScore = "";
                        tableMgr.UpdateMovieById(movie);

                        IEnumerable<string> reviewIds = movie.GetReviewIds();
                        foreach (string reviewId in reviewIds)
                        {
                            // Add code here
                        }

                        return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Queued rating reviews for this movie", ActualError = "" });
                    }
                    else
                    {
                        return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to find the movie", ActualError = "" });
                    }
                }
                else
                {
                    return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Pass in movie ID", ActualError = "" });
                }
            }
            else
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Pass in movie ID", ActualError = "" });
            }
        }
    }
}
