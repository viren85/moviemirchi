
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;

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
                string type = "all";
                int resultLimit = 15;

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    //getting type
                    type = qpParams["type"].ToString().ToLower();
                }

                if (!string.IsNullOrEmpty(qpParams["resultlimit"]))
                {
                    //getting result limit
                    resultLimit = Convert.ToInt32(qpParams["resultlimit"].ToString());
                }

                var tableMgr = new TableManager();

                var moviesByName = 
                    (type == "all") ?
                        tableMgr.GetSortedMoviesByName() :
                        (type == "current") ?
                            tableMgr.GetCurrentMovies() :
                            Enumerable.Empty<MovieEntity>();

                List<MovieEntity> movies = moviesByName.Take(resultLimit).ToList();

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
