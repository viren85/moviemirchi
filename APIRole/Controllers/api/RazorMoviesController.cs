
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// This API returns list of all the movies based on type. Type could be “current”, “all” “current” movies are having release date threshold of 1 month (configurable) 
    /// “all” movies type will return top 100 movies released recently.
    /// </summary>
    public class RazorMoviesController : ApiController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        public class Movie
        {
            public string Name { get; set; }
            public string UniqueName { get; set; }
            public string Synopsis { get; set; }
            public string MyScore { get; set; }
            public string Month { get; set; }
            public string Genre { get; set; }
            public string Rating { get; set; }
            public string Poster { get; set; }
            public bool IsTrailer { get; set; }
            public bool IsSong { get; set; }
            public bool IsPoster { get; set; }
        }

        // get : api/RazorMovies?type={current/all (default)}&resultlimit={default 100}          
        public object Get()
        {
            string type = "all";
            int resultLimit = 15;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    type = qpParams["type"].ToString().ToLower();
                }

                if (!string.IsNullOrEmpty(qpParams["resultlimit"]))
                {
                    int.TryParse(qpParams["resultlimit"].ToString(), out resultLimit);
                }
            }

            try
            {
                var tableMgr = new TableManager();

                var moviesByName =
                    (type == "all") ?
                        tableMgr.GetSortedMoviesByName() :
                        (type == "current") ?
                            tableMgr.GetCurrentMovies() :
                            (type == "upcoming") ?
                                tableMgr.GetUpcomingMovies() :
                                    Enumerable.Empty<MovieEntity>();

                List<Movie> movies = moviesByName.Take(resultLimit)
                    .Select(movie => {
                        var isSong = movie.Songs != null && movie.Songs.Contains("YoutubeURL");
                        var isTrailer = movie.Trailers != null && movie.Trailers.Contains("YoutubeURL");

                        var isPoster = false;
                        var poster = string.Empty;
                        if(movie.Posters != null && movie.Posters != "[]") {
                            var posters = JsonConvert.DeserializeObject<List<string>>(movie.Posters);
                            isPoster = posters.Count > 0;
                            poster = isPoster ? posters[0] : poster;
                        }

                        return new Movie()
                        {
                            Name = movie.Name,
                            UniqueName = movie.UniqueName,
                            Synopsis = movie.Synopsis,
                            MyScore = movie.MyScore,
                            Month = movie.Month,
                            Genre = movie.Genre,
                            Rating = movie.Rating,
                            Poster = poster,
                            IsPoster = isPoster,
                            IsTrailer = isTrailer,
                            IsSong = isSong
                        };
                    }).ToList();

                return movies;
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(
                   new
                   {
                       Status = "Error",
                       UserMessage = "Unable to get " + type + " movies",
                       ActualError = ex.Message
                   });
            }
        }
    }
}
