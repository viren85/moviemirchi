﻿
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Storage;
    using System;
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

            // get query string parameters
            string queryParameters = this.Request.RequestUri.Query;
            if (queryParameters != null)
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
                var tweets = tableMgr.GetRecentTweets(startIndex, pageSize);
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