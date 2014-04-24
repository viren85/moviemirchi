
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Summary: This API returns all the songs matching specified keywords.
    /// throw  : ArgumentException
    /// Return : If songs are found, JSON string contains list of songs with movie name, single poster, and song link and song title. This excludes any other movie information. 
    ///          Otherwise, empty string
    /// </summary>
    public class AllSongsController : BaseController
    {
        //get api/AllSongs?q={searchText}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["q"]))
                {
                    throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                }

                string searchSong = qpParams["q"];

                //get movie list based on song's title
                var movies = tableMgr.SearchSongs(searchSong);  //Collection of movie

                List<Songs> movieSongs = new List<Songs>();

                if (movies != null)
                {
                    // if movie list not null then get each movie's song
                    foreach (var movie in movies)
                    {
                        // deserialize movie songs 
                        List<Songs> songs = json.Deserialize(movie.Songs, typeof(Songs)) as List<Songs>;

                        if (songs != null)
                        {
                            foreach (var song in songs)
                            {
                                // add song object to movie songs list
                                movieSongs.Add(song);
                            }
                        }
                    }
                }

                // serialize songs list and then return.
                return json.Serialize(movieSongs);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_SEARCHING_SONGS, ActualError = ex.Message });
            }
        }
    }
}
