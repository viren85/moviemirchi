using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using DataStoreLib.Constants;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class GetActorsController : BaseController
    {   
        //Give the all unique actor name.
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                var tblMgr = new TableManager();

                //var movies = tblMgr.GetAllMovies();
                 var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParam["query"]))
                {
                    throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                }

                string actorName = qpParam["query"].ToString();

                // get movies by actor(roles like actor, actress, producer, director... etc) 
                var movies = tblMgr.SearchMoviesByActor(actorName);

                List<Object> allCast = new List<Object>();
                List<Cast> tempCast = new List<Cast>();
                int counter = 0;
                foreach (var movie in movies)
                {
                    List<Cast> castList = json.Deserialize(movie.Casts, typeof(List<Cast>)) as List<Cast>;
                    if (castList != null)
                    {
                       
                        foreach (var cast in castList)
                        {
                            if (!tempCast.Exists(c => c.name == cast.name))
                            {
                                tempCast.Add(cast);
                                allCast.Add(new { id = ++counter, name = cast.name });
                            }
                        }
                        
                    }
                }
                return json.Serialize(allCast);
            }
            catch (Exception ex)
            {
                return json.Serialize(new { staus = "Error", UserMessage = Constants.UM_WHILE_GETTING_CURRENT_MOVIES, ActualError = ex.Message });
                //throw new ArgumentException();
            }

        }
    }
}
