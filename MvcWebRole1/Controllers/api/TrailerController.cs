using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;

using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class TrailerController : BaseController
    {
        // get : api/Movies?type={current/all Trailer}        

        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            MovieInfo movieInfo = new MovieInfo();
            Multimedia mult = new Multimedia();

            try
            {
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["movieId"]))
                {
                    throw new ArgumentException("Movie not found.");
                }

                string movieId = qpParams["movieId"].ToString();
                var tableMgr = new TableManager();
                var movie = tableMgr.GetMovieById(movieId);

                if (movie != null)
                {
                    return movie.Trailers;
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
