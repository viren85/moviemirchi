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


namespace MvcWebRole1.Controllers.api
{
    public class AllSongsController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            if (string.IsNullOrEmpty(qpParams["searchSong"]))
            {
                throw new ArgumentException("Search Not found.");
            }
            string searchSong = qpParams["searchSong"];

            var tableMgr = new TableManager();
            var movieEntity = tableMgr.SearchSongs(searchSong);  //Collection of movie

            List<SongTrailer> movieSongs = new List<SongTrailer>();

            if (movieEntity != null)
            {
                foreach (var movie in movieEntity)
                {
                    List<SongTrailer> songs = json.Deserialize(movie.Songs, typeof(SongTrailer)) as List<SongTrailer>;

                    if (songs != null)
                    {
                        foreach (var song in songs)
                        {
                            movieSongs.Add(song);
                        }
                    }
                }
            }
            return json.Serialize(movieSongs);
        }
    }
}
