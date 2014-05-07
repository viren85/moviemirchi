
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
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            string type = "all";
            int resultLimit = 15;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    type = qpParams["type"].ToString().ToLower();
                }

                if (!string.IsNullOrEmpty(qpParams["resultlimit"]))
                {
                    int.TryParse(qpParams["resultlimit"].ToString(), out resultLimit);
                }
            }

            try
            {
                var tableMgr = new TableManager();

                var moviesByName =
                    (type == "all") ?
                        tableMgr.GetSortedMoviesByName() :
                        (type == "current") ?
                            tableMgr.GetCurrentMovies() :
                            (type == "upcoming") ?
                                tableMgr.GetUpcomingMovies() :
                                    Enumerable.Empty<MovieEntity>();

                List<MovieEntity> movies = moviesByName.Take(resultLimit).ToList();

                // serialize movieList object and return.
                return jsonSerializer.Value.Serialize(movies);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_CURRENT_MOVIES, ActualError = ex.Message });
            }
        }
    }
}
