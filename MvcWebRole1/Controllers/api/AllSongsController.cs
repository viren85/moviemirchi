using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web;
using DataStoreLib.Constants;


namespace MvcWebRole1.Controllers.api
{
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

                List<SongTrailer> movieSongs = new List<SongTrailer>();

                if (movies != null)
                {
                    // if movie list not null then get each movie's song
                    foreach (var movie in movies)
                    {
                        // deserialize movie songs 
                        List<SongTrailer> songs = json.Deserialize(movie.Songs, typeof(SongTrailer)) as List<SongTrailer>;

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
