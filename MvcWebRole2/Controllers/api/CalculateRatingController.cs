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

                    string movieId = string.Empty, reviewId = string.Empty, reviewText = string.Empty;

                    if (!string.IsNullOrEmpty(qpParams["movieid"]) && !string.IsNullOrEmpty(qpParams["reviewid"]))
                    {
                        movieId = qpParams["movieid"].ToString();
                        reviewId = qpParams["reviewid"].ToString();


                        var tableMgr = new TableManager();
                        var review = tableMgr.GetReviewById(reviewId);
                        if (review != null)
                        {
                            reviewText = review.Review;
                        }
                        else
                        {
                            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Unable to get the review", ActualError = "" });
                        }

                        try
                        {
                            //Execute exe file 
                            var callProcessReviewProc = new Process();
                            callProcessReviewProc.StartInfo = new ProcessStartInfo();

                            callProcessReviewProc.EnableRaisingEvents = false;
                            callProcessReviewProc.StartInfo.FileName = "cmd.exe";

                            string dirPath = @"e:\workspace";
                            string cmdPath = Path.Combine(dirPath, @"Scorer\scorer", "runScorer.cmd");
                            string filename = string.Format("{0}_{1}", movieId, reviewId);
                            string reviewFilename = Path.Combine(Path.GetTempPath(), filename + ".txt");
                            string logFilename = Path.Combine(Path.GetTempPath(), filename + ".log");
                            File.WriteAllText(reviewFilename, reviewText);

                            callProcessReviewProc.StartInfo.Arguments =
                                string.Format("/C {0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                                    cmdPath,
                                    dirPath,
                                    logFilename,
                                    movieId,
                                    reviewId,
                                    reviewFilename);

                            callProcessReviewProc.StartInfo.UseShellExecute = true;
                            callProcessReviewProc.Start();
                            callProcessReviewProc.WaitForExit();

                            return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMassege = "Successfully launch exe file" });
                        }
                        catch (Exception ex)
                        {
                            return jsonSerializer.Value.Serialize(new { Status = "Error", UserMassege = "Issue with executing the scorer script", ActualError = ex.Message });
                        }
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
