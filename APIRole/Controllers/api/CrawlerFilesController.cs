
namespace CloudMovie.APIRole.API
{
    using System;
    using DataStoreLib.Storage;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Configuration;
    using Crawler;

    /// <summary>
    /// This API returns list of all the movies details from the file on type.    
    /// </summary>
    public class CrawlerFilesController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static object _object = new object();


        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            try
            {
                // get query string parameters
                string queryParameters = this.Request.RequestUri.Query;

                if (queryParameters != null)
                {
                    var tableMgr = new TableManager();
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);

                    string movieInitials = string.Empty;
                    
                    if (!string.IsNullOrEmpty(qpParams["q"]))
                    {
                        movieInitials = qpParams["q"].ToString().ToLower();
                    }

                    var movieList = new List<XMLMovieProperties>();

                    if (string.IsNullOrEmpty(movieInitials))
                    {
                        // movie initials in empty then show all the movie from latest file
                        movieList = new XMLMovieProperties().GetMovieListFromXMLFiles(true);
                        return jsonSerializer.Value.Serialize(movieList);
                    }
                    else
                    {
                        // movie initials in not empty then show matched movie from all files
                        movieList = new XMLMovieProperties().GetMovieListFromXMLFiles(false);

                        var selectedMovies = movieList.FindAll(m => m.MovieName.ToLower().Contains(movieInitials.ToLower()));
                        return jsonSerializer.Value.Serialize(selectedMovies);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return string.Empty;
        }
    }
}
