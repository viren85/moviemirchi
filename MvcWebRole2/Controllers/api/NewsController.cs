
namespace MvcWebRole1.Controllers.api
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
            int startIndex = 0;
            int pageSize = 20;

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (!string.IsNullOrWhiteSpace(queryParameters))
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

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
                var tableMgr = new TableManager();
                var news = tableMgr.GetRecentNews(startIndex, pageSize);
                return jsonSerializer.Value.Serialize(news.Values);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_TWEETS, ActualError = ex.Message });
            }
        }
    }
}
