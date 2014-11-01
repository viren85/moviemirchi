using DataStoreLib.Constants;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CloudMovie.APIRole.API
{
    public class ArtistMoviesController : BaseController
    {

        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        private static Lazy<string> movieError = new Lazy<string>(() =>
                        jsonSerializer.Value.Serialize(
                            new
                            {
                                Status = "Error",
                                UserMessage = "Unable to get movies for artits.",
                                ActualError = "",
                            })
                        );

        private static Lazy<string> bioError = new Lazy<string>(() =>
                        jsonSerializer.Value.Serialize(
                            new
                            {
                                Status = "Error",
                                UserMessage = "Movie Mirchi is working on brief introduction about this artist, will be updated soon.",
                                ActualError = "",
                            })
                        );

        Func<string, string> UpdateArtistHit = (artistName) =>
        {
            int pageHit = 0;
            var tableMgr = new TableManager();
            var moviesByName = tableMgr.GetArtist(artistName);

            // Initially popularity was meant for most accessed celebrity. Now popularity is used to
            // keep track of celebrities who are most liked by end users
            if (!string.IsNullOrEmpty(moviesByName.JsonString))
            {
                // Read pagehit value & assign it to pagehit variable
                Dictionary<string, object> dict = (Dictionary<string, object>)jsonSerializer.Value.Deserialize(moviesByName.JsonString, typeof(object));
                pageHit++;
                int.TryParse(dict["PageHit"].ToString(), out pageHit);
                dict["PageHit"] = pageHit;
                moviesByName.JsonString = jsonSerializer.Value.Serialize((object)dict);
            }
            else
            {
                // increment the page hit count by 1
                //int.TryParse(json.FirstOrDefault().Value, out pageHit);
                pageHit++;
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("PageHit", pageHit);
                moviesByName.JsonString = jsonSerializer.Value.Serialize((object)dict);
            }

            tableMgr.UpdateArtistItemById(new List<ArtistEntity>() { moviesByName });

            return jsonSerializer.Value.Serialize(moviesByName);
        };

        // get : api/ArtistMovies?q=artist-name&page={default 30}
        protected override string ProcessRequest()
        {
            const int defaultMoviesCount = 30;
            int resultLimit = defaultMoviesCount;
            string artistName = string.Empty;
            string dataType = string.Empty;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["page"]))
                {
                    int.TryParse(qpParams["page"].ToString(), out resultLimit);
                }

                if (!string.IsNullOrEmpty(qpParams["name"]))
                {
                    artistName = qpParams["name"].ToLower().Trim();
                }

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    dataType = qpParams["type"].ToLower();
                }
            }

            try
            {
                var tableMgr = new TableManager();

                if (dataType == "movie")
                {
                    if (resultLimit == defaultMoviesCount)
                    {
                        string json;
                        string uArtistName = artistName.Replace(" ", "-").Replace(".", "-");
                        if (!CacheManager.TryGet<string>(CacheConstants.ArtistMoviesJson + uArtistName, out json))
                        {
                            var moviesByName = tableMgr.GetArtistMovies(artistName);
                            var movies = moviesByName.Take(resultLimit);
                            json = jsonSerializer.Value.Serialize(movies);
                            CacheManager.Add<string>(CacheConstants.ArtistMoviesJson + uArtistName, json);
                        }
                        return json;
                    }
                    else
                    {
                        var moviesByName = tableMgr.GetArtistMovies(artistName);
                        var movies = moviesByName.Take(resultLimit);
                        return jsonSerializer.Value.Serialize(movies);
                    }
                }
                else // Bio
                {
                    string json;
                    string uArtistName = artistName.Replace(" ", "-").Replace(".", "-");
                    if (!CacheManager.TryGet<string>(CacheConstants.ArtistBioJson + uArtistName, out json))
                    {
                        json = UpdateArtistHit(uArtistName);
                        CacheManager.Add<string>(CacheConstants.ArtistBioJson + uArtistName, json);
                    }
                    else
                    {
                        Task.Run(() => UpdateArtistHit(uArtistName));
                    }
                    return json;
                }
            }
            catch (Exception e)
            {
                // TODO: Log the error message
                // if any error occured then return User friendly message with system error message
                if (dataType == "movie")
                {
                    return movieError.Value;
                }
                else
                {
                    return bioError.Value;
                }
            }
        }
    }
}
