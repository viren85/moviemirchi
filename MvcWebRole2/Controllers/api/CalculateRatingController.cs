namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This api take movieId,reviewId,review text and call exe file which process the reviews and calculate it rating    
    /// </summary>
    public class CalculateRatingController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/CalculateRating?movieid=<mid>&reviewid=<rid>&review=<review Text>          
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

                    string movieId = string.Empty, reviewId = string.Empty, reviewText = string.Empty;

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]) && !string.IsNullOrEmpty(qpParams["review"]))
                    {
                        movieId = qpParams["movieid"].ToString();
                        reviewId = qpParams["reviewid"].ToString();
                        reviewText = qpParams["review"].ToString();

                        string fileName = Path.Combine(ConfigurationManager.AppSettings["ExeFilePath"], ConfigurationManager.AppSettings["ExeFileName"]);

                        //Execute exe file 
                        var callProcessReviewProc = new Process();
                        callProcessReviewProc.StartInfo = new ProcessStartInfo();

                        callProcessReviewProc.EnableRaisingEvents = false;
                        callProcessReviewProc.StartInfo.FileName = fileName;
                        callProcessReviewProc.StartInfo.Arguments = "\"" + movieId + "\" \"" + reviewId + "\" \"" + reviewText + "\"";
                        callProcessReviewProc.StartInfo.UseShellExecute = true;
                        callProcessReviewProc.Start();
                        callProcessReviewProc.WaitForExit();

                        return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMassege = "Successfully launch exe file" });
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
