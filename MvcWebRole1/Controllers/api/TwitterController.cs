
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
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            
            try
            {
                var tableMgr = new TableManager();
                var tweets = tableMgr.GetRecentTweets();
                return json.Serialize(tweets);
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_GETTING_TWEETS, ActualError = ex.Message });
            }
        }
    }
}
