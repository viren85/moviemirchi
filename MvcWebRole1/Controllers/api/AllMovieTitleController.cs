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
    public class AllMovieTitleController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrEmpty(qpParams["searchTitle"]))
            {
                throw new ArgumentException("Search Not found.");
            }
            string searchTitle = qpParams["searchTitle"];

            var tableMgr = new TableManager();
            var movieEntityTitle = tableMgr.SearchTitle(searchTitle);
           
            List<string> nameList = new List<string>();  // collection of names

            List<MovieInfo> movieTitleLIst = new List<MovieInfo>();  // collection of movie

            if (movieEntityTitle != null)
            {
                foreach (var movieTitle in movieEntityTitle)
                {
                    nameList.Add(movieTitle.Name);
                }
            }
            return json.Serialize(movieTitleLIst);
        }
    }
}
