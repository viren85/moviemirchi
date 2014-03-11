using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Script.Serialization;


namespace MvcWebRole1.Controllers.api
{
    public class ActorController : BaseController
    {
        // get : api/Actor?n={actor/actress/producer...etc name}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParam["n"]))
                {
                    throw new ArgumentException("Search Actor/Actress is not found");
                }

                string searchActor = qpParam["n"].ToString();

                var tableMgr = new TableManager();

                var movies = tableMgr.SearchMoviesByActor(searchActor);

                return json.Serialize(movies);
            }
            catch (Exception ex)
            {
                return json.Serialize(new { Status = "Error", UserMessage = "Error occured while getting movie's trailers", ActualError = ex.Message });
            }
        }
    }
}
