using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Script.Serialization;
using DataStoreLib.Utils;
using DataStoreLib.Storage;
using DataStoreLib.Models;

namespace MvcWebRole1.Controllers.api
{
    public class AllMovieTrailerController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            if (string.IsNullOrEmpty(qpParams["searchTrailer"]))
            {
                throw new ArgumentException("Search Not found.");
            }
            string searchTrailer = qpParams["searchTrailer"];

            var tableMgr = new TableManager();
            var movieEntity = tableMgr.SearchTrailer(searchTrailer);  // collection of movie

            List<SongTrailer> movieTrailers = new List<SongTrailer>();   

            if (movieEntity != null) 
            {
                foreach (var movie in movieEntity)
                {
                    List<SongTrailer> movi = json.Deserialize(movie.Trailers, typeof(SongTrailer)) as List<SongTrailer>;

                    if (movieTrailers != null)
                    {
                        foreach (var trailer in movi)
                        {
                            movieTrailers.Add(trailer);
                        }
                    }
                }
            }
            return json.Serialize(movieTrailers);
        }
    }
}
