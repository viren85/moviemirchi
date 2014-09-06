
namespace MvcWebRole2.Controllers.Library
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using MvcWebRole1.Controllers.api;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Web.Script.Serialization;

    internal abstract class Scorer
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        internal static string QueueScoreReview(string movieId, string reviewId)
        {
            string reviewText;
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
                    string.Format("/C {0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"",
                        cmdPath,
                        dirPath,
                        logFilename,
                        movieId,
                        reviewId,
                        reviewFilename,
                        "http://127.0.0.1:8080/");

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

        internal static string SetReviewAndUpdateMovieRating(string movieId, string reviewId, int rating, string bag)
        {
            var tableMgr = new TableManager();
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

                    var teekha = oldRating.teekharating + (rating > 0 ? 1 : 0);
                    var feekha = oldRating.feekharating + (rating < 0 ? 1 : 0);
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