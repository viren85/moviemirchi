using DataStoreLib.Models;

using DataStoreLib.Storage;
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
    public class SongController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            MovieInfo movieInfo = new MovieInfo();

            try
            {
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["movieId"]))
                {
                    throw new ArgumentException("Movie not Found");
                }

                string movieId = qpParams["movieId"].ToString();

                var tableMgr = new TableManager();
                var movie = tableMgr.GetMovieById(movieId);
                if (movie != null)
                {
                    return movie.Songs;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return string.Empty;
        }

    }
}
