using DataStoreLib.Constants;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    /// <summary>
    /// This API returns list of all the movies based on type. Type could be “current”, “all” “current” movies are having release date threshold of 1 month (configurable) 
    /// “all” movies type will return top 100 movies released recently.
    /// </summary>
    public class MoviesController : BaseController
    {
        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();
                var movies = new List<MovieEntity>();

                string type = "all";
                int resultLimit = 15;

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    //getting type
                    type = qpParams["type"].ToString();
                }

                if (!string.IsNullOrEmpty(qpParams["resultlimit"]))
                {
                    //getting resutl limit
                    resultLimit = Convert.ToInt32(qpParams["resultlimit"].ToString());
                }
                
                if (type.ToLower() == "all")
                {
                    // if type is "all" then get all movies 
                    var tempMovies = tableMgr.GetSortedMoviesByName();

                    if (tempMovies != null)
                    {
                        if (tempMovies.Count < resultLimit)
                        {
                            resultLimit = tempMovies.Count;
                        }

                        for (int i = 0; i < resultLimit; i++)
                        {
                            movies.Add(tempMovies[i]);
                        }
                    }
                }
                else if (type.ToLower() == "current")
                {
                    // if type is current then get current movies
                    var tempMovies = tableMgr.GetCurrentMovies();

                    if (tempMovies != null)
                    {
                        if (tempMovies.Count < resultLimit)
                        {
                            resultLimit = tempMovies.Count;
                        }

                        for (int i = 0; i < resultLimit; i++)
                        {
                            movies.Add(tempMovies[i]);
                        }
                    }
                }

                // serialize movieList object and return.
                return json.Serialize(movies);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_CURRENT_MOVIES, ActualError = ex.Message });
            }
        }
    }
}
