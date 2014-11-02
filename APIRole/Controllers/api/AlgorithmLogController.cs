
namespace CloudMovie.APIRole.API
{
    using CloudMovie.APIRole.Library;
    using DataStoreLib.Storage;
    using System;
    using System.Web;

    /// <summary>
    /// This API Accepts the physical path as query string, parses the path, scans the path and uploads the files to Blob  
    /// </summary>
    [Obsolete("Do not call this API, this is now integrated after the run for algorithm is scheduled and completed", true)]
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
                        Scorer.UploadAlgorithmRunLogs(physicalPath, reviewId);
                    }
                    else
                    {
                        return "Empty parameters";
                    }
                }
            }
            catch
            {
                throw;
            }

            return "Ok";
        }
    }
}
