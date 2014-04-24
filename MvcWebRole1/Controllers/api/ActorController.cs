
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Storage;
    using System;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns all the movies found based on actor name. e.g. Aamir Khan. This api will return list of movies which have aamir khan 
    /// (as actor, as singer, as director, as producer etc.)
    /// If movies are found, JSON string contains all movies information. This excludes reviews about these movies. Otherwise, empty string
    /// throw ArgumentException
    /// </summary>
    public class ActorController : BaseController
    {
        // get : api/Actor?n={actor/actress/producer...etc name}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();

                // get query string parameters
                var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParam["n"]))
                {
                    throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                }

                string actorName = qpParam["n"].ToString();

                // get movies by actor(roles like actor, actress, producer, director... etc) 
                var movies = tableMgr.SearchMoviesByActor(actorName);

                // serialize movie list and return
                return json.Serialize(movies);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_SEARCHING_MOVIES, ActualError = ex.Message });
            }
        }
    }
}
