﻿
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Storage;
    using System;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Summary: This API returns all the posters/pictures found for specified movie.
    /// throw  : ArgumentException
    /// Return : If posters are found, JSON string contains list of posters/pictures for the movie. This excludes any other movie information. Otherwise, empty string
    /// </summary>    
    public class PosterController : BaseController
    {
        //GET api/Poster?n={movie name}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            
            try
            {
                var tableMgr = new TableManager();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["n"]))
                {
                    throw new ArgumentException(Constants.API_EXC_MOVIE_NAME_NOT_EXIST);
                }

                string movieUniqueName = qpParams["n"].ToString();

                //get movie object by movie's unique name
                var movie = tableMgr.GetMovieByUniqueName(movieUniqueName);

                if (movie != null)
                {
                    // if movie is not null then return poster string (in json)
                    return movie.Posters;
                }
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_SEARCHING_MOVIES_POSTER, ActualError = ex.Message });
            }

            // if movie is null then return single object.
            return string.Empty;
        }
    }
}
