
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns single movie object based on the movie name.
    /// If movie result found, return JSON string contains, all the movie information including reviews. Otherwise, return empty string.
    /// throw ArgumentException
    /// </summary>
    public class TwitterController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/twitter?start=0&page=20
        protected override string ProcessRequest()
        {
            int startIndex = 0;
            int pageSize = 20;
            string name = string.Empty;
            string tweetType = string.Empty;

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
                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    tweetType = qpParams["type"].ToString();
                }
                if (!string.IsNullOrEmpty(qpParams["name"]))
                {
                    name = qpParams["name"].ToString();
                }
            }

            try
            {
                var tableMgr = new TableManager();
                IDictionary<string, TwitterEntity> tweets = null;
                if (string.IsNullOrEmpty(tweetType))
                {
                    tweets = tableMgr.GetRecentTweets(startIndex, pageSize);
                }
                else
                {
                    tweets = tableMgr.GetRecentTweets(tweetType, name, startIndex, pageSize);
                }

                return jsonSerializer.Value.Serialize(tweets);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_TWEETS, ActualError = ex.Message });
            }
        }
    }
}
