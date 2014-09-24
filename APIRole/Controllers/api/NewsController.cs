
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns top 20 News items based on time
    /// </summary>
    public class NewsController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/news?start=0&page=20
        protected override string ProcessRequest()
        {
            var tableMgr = new TableManager();

            int startIndex = 0;
            int pageSize = 20;
            string mode = "get";
            string newsId = string.Empty;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;

            if (!string.IsNullOrWhiteSpace(queryParameters))
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["mode"]))
                {
                    mode = qpParams["mode"].ToString().ToLower();
                }

                if (!string.IsNullOrEmpty(qpParams["id"]))
                {
                    newsId = qpParams["id"].ToString();
                }

                if (!string.IsNullOrEmpty(qpParams["start"]))
                {
                    int.TryParse(qpParams["start"].ToString(), out startIndex);
                }

                if (!string.IsNullOrEmpty(qpParams["page"]))
                {
                    int.TryParse(qpParams["page"].ToString(), out pageSize);
                }
            }

            try
            {
                if (mode == "get")
                {                    
                    var news = tableMgr.GetRecentNews(startIndex, pageSize);
                    return jsonSerializer.Value.Serialize(news);
                }
                else if (mode == "delete")
                {
                    
                }

                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "News-id does not empty!" });
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_TWEETS, ActualError = ex.Message });
            }
        }
    }
}
