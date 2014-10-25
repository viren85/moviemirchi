
namespace CloudMovie.APIRole.API
{
    using Crawler;
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API Accepts the physical path as query string, parses the path, scans the path and uploads the files to Blob  
    /// </summary>
    public class AlgorithmLogController : BaseController
    {

        protected override string ProcessRequest()
        {
            try
            {
                string queryParameters = this.Request.RequestUri.Query;
                TableManager tm = new TableManager();
                if (queryParameters != null)
                {
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);
                    string physicalPath = string.Empty;
                    string reviewId = string.Empty;

                    if (!string.IsNullOrEmpty(qpParams["p"]) && !string.IsNullOrEmpty(qpParams["id"]))
                    {
                        physicalPath = qpParams["p"].ToString().ToLower().Trim();
                        reviewId = qpParams["id"].ToString();
                    }

                    string blobPath = Util.UploadLogFile(physicalPath);
                    ReviewEntity re = tm.GetReviewById(reviewId);
                    re.AlgoLogUrl = blobPath;
                    tm.UpdateReviewById(re);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return string.Empty;
        }
    }
}
