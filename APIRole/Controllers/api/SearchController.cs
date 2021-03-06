﻿
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Storage;
    using System;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns all the movies found based on search query.
    /// throw ArgumentException
    /// </summary>
    public class SearchController : BaseController
    {
        // get : api/Search?q={searchText}        
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["q"]))
                {
                    throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                }

                string searchText = qpParams["q"];

                searchText = string.IsNullOrEmpty(searchText) ? string.Empty : searchText.Replace(".", "");

                // get movies by search keyword
                var movie = tableMgr.SearchMovies(searchText);

                // serialize movie list and return
                return json.Serialize(movie);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with sys  tem error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_SEARCHING_MOVIES, ActualError = ex.Message });
            }
        }
    }
}
