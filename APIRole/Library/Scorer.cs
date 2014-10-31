
namespace CloudMovie.APIRole.Library
{
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
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
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Unable to get the review", ActualError = "" });
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
                        "http://127.0.0.1:8081/");

                callProcessReviewProc.StartInfo.UseShellExecute = true;
                callProcessReviewProc.Start();
                callProcessReviewProc.WaitForExit();

                // TODO: This is not quite wired correctly, revisit this
                // We say WaitForExit, and that is why we can guarantee that the processing is completed, and log file is ready for read.
                // Somehow write this through bag of words that we would receive in SetReviewAndUpdateMovieRating method.
                Scorer.SetTagsForReview(reviewId, logFilename);

                return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMessage = "Successfully launch exe file" });
            }
            catch (Exception ex)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Issue with executing the scorer script", ActualError = ex.Message });
            }
        }

        internal static void SetTagsForReview(string reviewId, string filePath)
        {
            var lines = File.ReadAllLines(filePath);


            // Input:Sentiment: thumbsdown
            var sentiment =
                lines
                    .First(line => line.StartsWith("Sentiment: "))
                    .Replace("Sentiment: ", "")
                    .Trim();

            // Input:	Word: drama 	POS-tagger:  NN 	POS-SWN:  n 	Tag: POS 	Sentiment:  0.13774104683195595 	DebugString: null
            var terms =
                lines
                    .Where(line => line.Contains("POS-tagger:"))
                    .Select(line => line.Split('\t')
                        .Skip(1)
                        .Select(l => l.Trim()
                            .Split(':')
                            .Select(ll => ll.Trim())
                            .ToArray())
                        .ToDictionary(l => l[0], l => l[1]))
                    .ToList();

            terms.Sort((a, b) => double.Parse(a["Sentiment"]).CompareTo(double.Parse(b["Sentiment"])));

            var pos = terms
                .Take(sentiment == "thumbsdown" ? 6 : 4);
            var neg = terms.
                Skip(Math.Max(0, terms.Count - (sentiment == "thumbsup" ? 6 : 4)))
                .Take(sentiment == "thumbsup" ? 6 : 4);

            var tags =
                string.Join(" ",
                    pos
                        .Concat(neg)
                        .Select(term => term["Word"]));

            var tableMgr = new TableManager();
            ReviewEntity review = tableMgr.GetReviewById(reviewId);
            if (review != null)
            {
                review.Tags = tags;
                tableMgr.UpdateReviewById(review);
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
                    movie.Rating = newRating.criticrating;
                    movie.MyScore = strNewRating;
                    tableMgr.UpdateMovieById(movie);

                    return jsonSerializer.Value.Serialize(new { Status = "Ok", UserMessage = "Successfully update movie rating" });
                }
                else
                {
                    return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Unable to find review with passed review id. Please check review id." });
                }
            }
            else
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Unable to find movie with passed movie id. Please check movie id." });
            }
        }
    }
}