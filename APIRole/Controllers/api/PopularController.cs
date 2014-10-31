
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class PopularController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static Lazy<string> jsonError = new Lazy<string>(() =>
            jsonSerializer.Value.Serialize(
                new
                {
                    Status = "Error",
                    UserMessage = "Unable to get popular tags.",
                    ActualError = "",
                })
            );

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Prepare JSON object like:
        ///     [
        ///         [
        ///             { "UniqueName": "taran-adarsh", "Name": "Taran Adarsh", "Role": "Reviewer", "Weight": "3" },
        ///             { "UniqueName": "anupama-chopra", "Name": "Anupama Chopra", "Role": "Reviewer", "Weight": "4" },
        ///             { "UniqueName": "rachit-gupta", "Name": "Rachit Gupta", "Role": "Reviewer", "Weight": "2" }
        ///         ],
        ///         [
        ///             { "UniqueName": "mickey-virus", "Name": "Mickey Virus", "Role": "Movie", "Weight": "1" },
        ///             { "UniqueName": "krrish-3", "Name": "Krrish 3", "Role": "Movie", "Weight": "4" }
        ///         ],
        ///         [
        ///             { "UniqueName": "Deepika-Padukone", "Name": "Deepika Padukone", "Role": "Artists", "Weight": "5" },
        ///             { "UniqueName": "ranveer-singh", "Name": "Ranveer Singh", "Role": "Artists", "Weight": "4" },
        ///             { "UniqueName": "aditya-roy-kapoor", "Name": "Aditya Roy Kapoor", "Role": "Artists", "Weight": "1" },
        ///             { "UniqueName": "sanjay-leela-bhansali", "Name": "Sanjay Leela Bhansali", "Role": "Artists", "Weight": "2" }
        ///         ],
        ///         [
        ///             { "UniqueName": "Romance", "Name": "Romance", "Role": "Genre", "Weight": "5" },
        ///             { "UniqueName": "Action", "Name": "Action", "Role": "Genre", "Weight": "3" },
        ///             { "UniqueName": "Drama", "Name": "Drama", "Role": "Genre", "Weight": "1" }
        ///         ]
        ///     ]
        /// </remarks>
        /// <returns></returns>
        protected override string ProcessRequest()
        {
            string popularTags;
            if (!CacheManager.TryGet<string>(CacheConstants.PopularTagsJson, out popularTags))
            {
                try
                {
                    var tableMgr = new TableManager();
                    List<MovieEntity> movieEntities =
                        tableMgr.GetUpcomingMovies()
                        .Concat(tableMgr.GetCurrentMovies())
                        .ToList();

                    //int count = movieEntities.Count;
                    int count = 6; //set default 6 if we do not specify a value in web.config file

                    int.TryParse(ConfigurationManager.AppSettings["PopulerMovieCount"], out count);

                    var roleReviewer = new string[] { "Taran Adarsh", "Anupama Chopra", "Rajeev Masand" }
                        .Select(r => new
                        {
                            UniqueName = r.ToLower().Replace(" ", "-"),
                            Name = r,
                            Role = "Reviewer",
                            Weight = "3",
                        });

                    var roleMovie = movieEntities
                        .OrderByDescending(m => m.Name)
                        .Select(m => new
                        {
                            UniqueName = m.UniqueName,
                            Name = m.Name,
                            Role = "Movie",
                            Weight = "3",
                        })
                        .Take(count); //count/3

                    // TODO Fix Actors
                    #region Old code for getting actors in trending section
                    /*var actor = movieEntities
                        .SelectMany(m => m.GetActors())
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(3);

                    var artist = movieEntities
                        .SelectMany(m =>
                            m.GetDirectors()
                            .Concat(m.GetMusicDirectors())
                            .Concat(m.GetProducers()))
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(2);

                    var roleArtist = artist
                        .Concat(actor)
                        .Select(a => new
                        {
                            UniqueName = a.Key.ToLower().Replace(" ", "-"),
                            Name = a.Key,
                            Role = "Artists",
                            Weight = a.Count(),
                        });
                    */
                    #endregion

                    var actor = movieEntities
                        .SelectMany(m => m.GetActors(jsonSerializer.Value.Deserialize<List<Cast>>(m.Cast)))
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(3);

                    var artist = movieEntities
                        .SelectMany(m =>
                            m.GetDirectors()
                            .Concat(m.GetMusicDirectors())
                            .Concat(m.GetProducers()))
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(2);

                    //var roleArtist = artist
                    //.Concat(actor)
                    var roleArtist = actor
                        .Select(a => new
                        {
                            UniqueName = a.Key.ToLower().Replace(" ", "-"),
                            Name = a.Key,
                            Role = "Artists",
                            Weight = a.Count(),
                        });


                    var roleGenre = movieEntities
                        .SelectMany(m => m.Genre.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries))
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(3)
                        .Select(g => new
                        {
                            UniqueName = g.Key.ToLower().Replace(" ", "-"),
                            Name = g.Key,
                            Role = "Genre",
                            Weight = g.Count(),
                        });


                    var popular = string.Format(
                        "[{0},{1},{2},{3}]",
                        ////"[{0},{1},{2},{3}]",
                        jsonSerializer.Value.Serialize(roleMovie),
                        jsonSerializer.Value.Serialize(roleArtist),
                        jsonSerializer.Value.Serialize(roleReviewer),
                        jsonSerializer.Value.Serialize(roleGenre));

                    popularTags = popular;

                    CacheManager.Add<string>(CacheConstants.PopularTagsJson, popularTags);
                }
                catch (Exception)
                {
                    // if any error occured then return User friendly message with system error message
                    return jsonError.Value;
                }
            }

            return popularTags;
        }
    }
}
