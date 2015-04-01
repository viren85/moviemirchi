
namespace CloudMovie.APIRole.Controllers.api
{
    using CloudMovie.APIRole.API;
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using MovieCrawler;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class CrawlBingImagesController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
               
                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                if (string.IsNullOrEmpty(qpParams["n"]))
                {
                    throw new ArgumentException(Constants.API_EXC_MOVIE_NAME_NOT_EXIST);
                }

                // get the bing images url
                string searchTerm = qpParams["n"].ToString();
                string bingURL = "http://www.bing.com/images/search?&q=" + searchTerm;

               // string bingURL = "http://www.bing.com/images/search?sid=E8BCC6199DA2412BA5030B63AD02B439&q=" + searchTerm + "&qft=+filterui:license-L2_L3_L4&FORM=R5IR39";

                List<string> bingPosters = new BingCrawler().GetMoviePosterUrls(bingURL);

                if (bingPosters != null)
                {
                    
                    return  json.Serialize(bingPosters);
                }
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = "Error occured while getting posters", ActualError = ex.Message });
            }

            // if movie is null then return single object.
            return string.Empty;
        }

    }
}
