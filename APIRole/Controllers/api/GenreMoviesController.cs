using DataStoreLib.Constants;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CloudMovie.APIRole.API
{
    public class GenreMoviesController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        private static Lazy<string> jsonError = new Lazy<string>(() =>
            jsonSerializer.Value.Serialize(
                new
                {
                    Status = "Error",
                    UserMessage = "Unable to get movies by genre.",
                    ActualError = "",
                })
            );

        // get : api/GenreMovies?q=artist-name&page={default 30}
        protected override string ProcessRequest()
        {
            int resultLimit = 30;
            string genre = string.Empty;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["page"]))
                {
                    int.TryParse(qpParams["page"].ToString(), out resultLimit);
                }

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    genre = qpParams["type"].ToLower();
                }
            }

            try
            {
                var tableMgr = new TableManager();
                var moviesByName = tableMgr.GetGenrewiseMovies(genre);
                List<MovieEntity> movies = moviesByName.Take(resultLimit).ToList();
                return jsonSerializer.Value.Serialize(movies);
            }
            catch (Exception)
            {
                // if any error occured then return User friendly message with system error message
                return jsonError.Value;
            }
        }
    }
}
