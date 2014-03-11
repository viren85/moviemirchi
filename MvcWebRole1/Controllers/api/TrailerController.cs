using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;

using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class TrailerController : BaseController
    {
        // get : api/Trailer?n={movie's unique name}        

        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["n"]))
                {
                    throw new ArgumentException("Movie name is not supplied.");
                }

                string movieUniqueName = qpParams["n"].ToString();

                var tableMgr = new TableManager();
                var movie = tableMgr.GetMovieByUniqueName(movieUniqueName);

                if (movie != null)
                {
                    return movie.Trailers;
                }
            }
            catch (Exception ex)
            {
                return json.Serialize(new { Status = "Error", UserMessage = "Error occured while getting movie's trailers", ActualError = ex.Message });
            }

            return string.Empty;
        }
    }

}
