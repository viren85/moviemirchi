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
    public class MoviesController : BaseController
    {
        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                SetConnectionString();

                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                string type = "all";
                int resultLimit = 100;

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    type = qpParams["type"].ToString();
                }

                if (!string.IsNullOrEmpty(qpParams["resultlimit"]))
                {
                    resultLimit = Convert.ToInt32(qpParams["resultlimit"].ToString());
                }

                var tableMgr = new TableManager();

                var movies = new List<MovieEntity>();

                if (type.ToLower() == "all")
                {
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

                return json.Serialize(movies);
            }
            catch (Exception ex)
            {
                return json.Serialize(new { Status = "Error", Message = ex.Message });
            }
        }

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
    }
}
