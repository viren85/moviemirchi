

namespace CloudMovie.APIRole.Controllers.api
{
    using CloudMovie.APIRole.API;
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.BlobStorage;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Twitterizer;

    public class CrawlTweetsController : BaseController
    {
        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ActionResult GetTweets()
        {
            try
            {
                try
                {
                    BlobStorageService _blobStorageService = new BlobStorageService();
                    string twitXmlBlobFilePath = _blobStorageService.GetSinglFile(BlobStorageService.Blob_XMLFileContainer, "Twitter.xml");
                    GetTweets(twitXmlBlobFilePath);
                }
                catch (Exception)
                {
                    //return Json(new { Status = "Error", Message = "Error occured.", ActualMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

            }

            return null;
            //return Json(new { Status = "Ok", Message = "Selected news deleted successfully." }, JsonRequestBehavior.AllowGet);
        }

        
        private void GetTweets(string blobXmlFilePath = "")
        {
            try
            {
                string consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
                string consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
                List<string> tweetIdList = new List<string>();
                OAuthTokens oaccesstkn = new OAuthTokens();
                oaccesstkn.AccessToken = ConfigurationManager.AppSettings["TwitterAccessToken"];
                oaccesstkn.AccessTokenSecret = ConfigurationManager.AppSettings["TwitterAccessSecret"]; ;
                oaccesstkn.ConsumerKey = consumerKey;
                oaccesstkn.ConsumerSecret = consumerSecret;

                XmlDocument xdoc = new XmlDocument();

                if (blobXmlFilePath == "")
                {
                    string basePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                    string filePath = Path.Combine(basePath, "Twitter.xml");

                    xdoc.Load(filePath);
                }
                else
                {
                    xdoc.Load(blobXmlFilePath);
                }

                var items = xdoc.SelectNodes("Search/WhiteList/Item");

                if (items != null)
                {
                    foreach (XmlNode item in items)
                    {
                        WebRequestBuilder webRequest = new WebRequestBuilder(new Uri(string.Format(ConfigurationManager.AppSettings["TwitterUrl"], item.InnerText, 500)), HTTPVerb.GET, oaccesstkn);

                        string responseText;
                        using (var response = webRequest.ExecuteRequest())
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseText = reader.ReadToEnd();
                            }
                        }

                        var brr = Encoding.UTF8.GetBytes(responseText);

                        var streamReader = new StreamReader(new MemoryStream(brr));

                        var serializer = new DataContractJsonSerializer(typeof(SearchResults));

                        var tweetsResponse = (SearchResults)serializer.ReadObject(streamReader.BaseStream);

                        streamReader.Close();
                        TableManager tblMgr = new TableManager();
                        foreach (var mmTweetData in tweetsResponse.Results)
                        {
                            var tweetId = mmTweetData.Id.ToString();
                            tweetIdList = new List<string>();
                            tweetIdList.Add(tweetId);

                            if (!tblMgr.IsTweetExist(tweetIdList))
                            {

                                var myTweet = new TwitterEntity
                                {
                                    RowKey = Guid.NewGuid().ToString(),
                                    TwitterId = tweetId,
                                    TwitterIdString = mmTweetData.Id_Str,
                                    TextMessage = mmTweetData.Text,
                                    Source = mmTweetData.Source,
                                    FromUser = mmTweetData.Source,
                                    FromUserId = mmTweetData.ToUserName,
                                    ProfileImageUrl = mmTweetData.User.ProfileImageUrl,
                                    ProfileSecureImageUrl = mmTweetData.User.ProfileImageUrlHttps,
                                    ReplyUserId = mmTweetData.User.FromUserId.ToString(),
                                    ReplyScreenName = mmTweetData.User.FromUser,
                                    ResultType = mmTweetData.SearchMetaData.ResultType,
                                    LanguageCode = mmTweetData.SearchMetaData.IsoLanguageCode,
                                    Created_At = ParseTwitterDateTime(mmTweetData.CreatedAt),
                                    Status = "-1",
                                    TweetType = item.Attributes["type"].Value,
                                    MovieName = item.Attributes["movie-name"] != null ? item.Attributes["movie-name"].Value : string.Empty,
                                    ArtistName = item.Attributes["artist-name"] != null ? item.Attributes["artist-name"].Value : string.Empty
                                };

                                tblMgr.UpdateTweetById(myTweet);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private DateTime ParseTwitterDateTime(string date)
        {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }
    }
}
