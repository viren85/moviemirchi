
namespace CloudMovie.APIRole.API
{
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns list of all the movies based on type. Type could be “current”, “all” “current” movies are having release date threshold of 1 month (configurable) 
    /// “all” movies type will return top 100 movies released recently.
    /// </summary>
    public class MovieController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [AcceptVerbs("POST")]
        public ActionResult AddMovie(MoviePostData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.UniqueName))
            {
                return null;
            }

            try
            {
                var tableMgr = new TableManager();
                MovieEntity movie = data.GetMovieEntity();
                movie.RowKey = movie.MovieId;
                tableMgr.UpdateMovieById(movie);

                UpdateCache(movie);

                UpdateLuceneIndex(movie);
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        private static void UpdateLuceneIndex(MovieEntity movie)
        {
            var tableMgr = new TableManager();

            // Update Lucene
            Task.Run(() =>
            {
                //delete Entry in lucene search index
                // Fix following method call - What shall be other param? 
                LuceneSearch.ClearLuceneIndexRecord(movie.MovieId, "Id");
                LuceneSearch.ClearLuceneIndexRecord(movie.UniqueName, "UniqueName");

                string posterUrl = "default-movie.jpg";
                string critics = string.Empty;

                if (!string.IsNullOrEmpty(movie.Posters))
                {
                    List<string> pList = jsonSerializer.Value.Deserialize(movie.Posters, typeof(List<string>)) as List<string>;
                    if (pList != null && pList.Count > 0)
                    {
                        posterUrl = pList.Last();
                    }
                }

                var reviewDic = tableMgr.GetReviewsByMovieId(movie.MovieId);
                if (reviewDic != null && reviewDic.Values != null && reviewDic.Values.Count > 0)
                {
                    critics = jsonSerializer.Value.Serialize(reviewDic.Values.Select(re => re.ReviewerName));
                }

                // add updated entry in lucene search index
                MovieSearchData movieSearchIndex = new MovieSearchData();
                movieSearchIndex.Id = movie.RowKey;
                movieSearchIndex.Title = movie.Name;
                movieSearchIndex.Type = movie.Genre;
                movieSearchIndex.TitleImageURL = posterUrl;
                movieSearchIndex.UniqueName = movie.UniqueName;
                movieSearchIndex.Description = movie.Cast;
                movieSearchIndex.Critics = critics;
                movieSearchIndex.Link = movie.UniqueName;

                LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
            });
        }

        private static void UpdateCache(MovieEntity movie)
        {
            // Remove movie from Cache
            var movieKey = CacheConstants.MovieInfoJson + movie.UniqueName;
            var isPreviouslyCached = CacheManager.Exists(movieKey);
            CacheManager.Remove(movieKey);

            var tableMgr = new TableManager();

            // Cache if previously cached or movie is upcoming/current
            Task.Run(() =>
            {
                if (isPreviouslyCached || movie.State == "upcoming" || movie.State == "now playing")
                {
                    MovieInfoController.GetMovieInfo(movie.UniqueName);
                }
            });

            // Update more Cache
            Task.Run(() =>
            {
                // Update cache for AllMovies
                // Note: We are not updating CacheConstants.AllMovieEntitiesSortedByName here
                // because typically the name of the movie does not changes as often
                CacheManager.Remove(CacheConstants.AllMovieEntities);
                tableMgr.GetAllMovies();

                // Update current movies
                Task.Run(() =>
                {
                    CacheManager.Remove(CacheConstants.UpcomingMovieEntities);
                    var movies = tableMgr.GetCurrentMovies();
                });

                // Update upcoming movies
                Task.Run(() =>
                {
                    CacheManager.Remove(CacheConstants.NowPlayingMovieEntities);
                    var movies = tableMgr.GetUpcomingMovies();
                });
            });
        }
    }
}
