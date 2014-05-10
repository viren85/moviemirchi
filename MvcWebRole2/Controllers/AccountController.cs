using Crawler;
using Crawler.Reviews;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using LuceneSearchLibrarby;
using Microsoft.WindowsAzure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using Twitterizer;


namespace MvcWebRole2.Controllers
{
    public class AccountController : Controller
    {
        #region Set Connection String
        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        #endregion

        #region Login
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["userid"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Please enter username and password";
                return View();
            }

            try
            {
                SetConnectionString();

                TableManager tblMgr = new TableManager();
                UserEntity entity = tblMgr.GetUserByName(userName);
                if (entity != null)
                {
                    if (entity.UserName == userName && entity.Password == password)
                    {
                        Session["user"] = entity.UserName;
                        Session["userid"] = entity.UserId;
                        Session["username"] = entity.FirstName + " " + entity.LastName;
                        Session["type"] = entity.UserType;

                        return RedirectToAction("AddMovie", "Movie");
                    }
                    else
                    {
                        TempData["Error"] = "Login Failed. Inalid username or password.";
                    }
                }
                else
                {
                    TempData["Error"] = "Login Failed. Inalid username or password.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Login Failed. Inalid username or password.";
            }

            return View();
        }
        #endregion

        #region Sign up
        [HttpGet]
        public ActionResult Register()
        {
            if (Session["userid"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public void Crawl()
        {
            Crawler.MovieCrawler movieCrawler = new Crawler.MovieCrawler();
            JavaScriptSerializer json = new JavaScriptSerializer();

            SetConnectionString();
            CreatePosterDirectory();

            try
            {
                XmlDocument xdoc = new XmlDocument();

                string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);

                string[] moviesFilePath = Directory.GetFiles(basePath, "*.xml");

                #region Movie Crawler
                foreach (string filePath in moviesFilePath)
                {
                    xdoc.Load(filePath);

                    var movies = xdoc.SelectNodes("Movies/Month[@name='October']/Movie");

                    if (movies == null)
                        continue;

                    foreach (XmlNode movie in movies)
                    {
                        if (movie.Attributes["link"] != null && !string.IsNullOrEmpty(movie.Attributes["link"].Value))
                        {
                            try
                            {

                                #region Crawl Movie
                                MovieEntity mov = movieCrawler.Crawl(movie.Attributes["link"].Value);
                                TableManager tblMgr = new TableManager();
                                string posterUrl = string.Empty;

                                tblMgr.UpdateMovieById(mov);
                                #endregion

                                #region Crawl Movie Reviews
                                #region Bollywood Hungama Crawler
                                try
                                {
                                    BollywoodHungamaReviews bh = new BollywoodHungamaReviews();
                                    HindustanTimesReviews ht = new HindustanTimesReviews();
                                    FilmfareReviews ff = new FilmfareReviews();

                                    var reviews = movie.SelectNodes("Review");
                                    foreach (XmlNode review in reviews)
                                    {
                                        ReviewEntity re = new ReviewEntity();
                                        string reviewLink = review.Attributes["link"].Value;

                                        switch (review.Attributes["name"].Value)
                                        {
                                            case "Bollywood Hungama":
                                                re = bh.Crawl(reviewLink, review.Attributes["name"].Value);
                                                break;
                                            case "Hindustan Times":
                                                re = ht.Crawl(reviewLink, review.Attributes["name"].Value);
                                                break;
                                            case "Filmfare":
                                                re = ff.Crawl(reviewLink, review.Attributes["name"].Value);
                                                break;
                                        }

                                        // update the IDs - Movie Id, Reviewer Id etc.
                                        string reviewerId = ReviewCrawler.SetReviewer(re.ReviewerName, review.Attributes["name"].Value);
                                        //re.RowKey = re.ReviewId = new Guid().ToString();
                                        re.ReviewerId = reviewerId;
                                        re.MovieId = mov.MovieId;
                                        re.OutLink = reviewLink;
                                        tblMgr.UpdateReviewById(re);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                #endregion
                                #endregion

                                #region Lucene Search Index
                                List<Cast> casts = json.Deserialize(mov.Casts, typeof(List<Cast>)) as List<Cast>;
                                List<String> posters = json.Deserialize(mov.Posters, typeof(List<String>)) as List<String>;
                                List<String> actors = new List<string>();

                                if (casts != null)
                                {
                                    foreach (var actor in casts)
                                    {
                                        actors.Add(actor.name);
                                    }
                                }

                                if (posters != null && posters.Count > 0)
                                {
                                    posterUrl = posters[posters.Count - 1];
                                }

                                // include reviewer & their affiliation in index file
                                MovieSearchData movieSearchIndex = new MovieSearchData();
                                movieSearchIndex.Id = mov.RowKey;
                                movieSearchIndex.Title = mov.Name;
                                movieSearchIndex.Type = mov.Genre;
                                movieSearchIndex.TitleImageURL = posterUrl;
                                movieSearchIndex.UniqueName = mov.UniqueName;
                                movieSearchIndex.Description = json.Serialize(actors);
                                movieSearchIndex.Link = mov.UniqueName;
                                LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
                                #endregion

                            }
                            catch (Exception)
                            {
                                Debug.WriteLine("Error while crawling movie - " + movie.Attributes["link"].Value);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: {0}", ex);
                throw;
            }

        }

        [HttpGet]
        public void GetTweets()
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

                string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                string filePath = Path.Combine(basePath, "Twitter.xml");

                xdoc.Load(filePath);
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

                        #region Filter Blocked List of Twitter Handles
                        /*string blackList = ConfigurationManager.AppSettings["BlockedTwitterHandles"];
                        Hashtable twitterAccounts = new Hashtable();
                        if (!string.IsNullOrEmpty(blackList))
                        {
                            string[] accounts = blackList.Split(',');
                            foreach (string account in accounts)
                            {
                                twitterAccounts.Add(account.Trim().ToLower(), account.Trim().ToLower());
                            }
                        }*/
                        #endregion
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
                                    Status = "-1"
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

        [HttpGet]
        public void GetNews()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();

                string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                string filePath = Path.Combine(basePath, "News.xml");
                string newsXml = string.Empty;

                xdoc.Load(filePath);
                var items = xdoc.SelectSingleNode("News/Link");
                if (items != null)
                {
                    foreach (XmlNode item in items)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.InnerText);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            #region Get Review Page Content
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;
                            if (response.CharacterSet == null)
                                readStream = new StreamReader(receiveStream);
                            else
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                            newsXml = readStream.ReadToEnd();
                            List<NewsEntity> news = ParseNewsItems(newsXml, item.ParentNode.Attributes["type"].Value);
                            TableManager tblMgr = new TableManager();
                            tblMgr.UpdateNewsById(news);
                            response.Close();
                            readStream.Close();
                            #endregion
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        [HttpPost]
        public ActionResult Register(string userJson)
        {
            if (string.IsNullOrEmpty(userJson))
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();

                UserEntity deUser = json.Deserialize(userJson, typeof(UserEntity)) as UserEntity;

                if (deUser != null)
                {
                    try
                    {
                        System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(deUser.Email);
                    }
                    catch (Exception)
                    {
                        return Json(new { Status = "Error", Message = "Please provide valid email address." }, JsonRequestBehavior.AllowGet);
                    }

                    if (deUser.Password != deUser.Mobile)
                    {
                        return Json(new { Status = "Error", Message = "Password and confirm password does not match." }, JsonRequestBehavior.AllowGet);
                    }

                    SetConnectionString();

                    TableManager tblMgr = new TableManager();

                    UserEntity oldUser = tblMgr.GetUserByName(deUser.UserName);

                    if (oldUser == null)
                    {
                        UserEntity entity = new UserEntity();
                        entity.RowKey = entity.UserId = Guid.NewGuid().ToString();
                        entity.UserName = deUser.UserName;
                        entity.Password = deUser.Password;
                        entity.FirstName = deUser.FirstName;
                        entity.LastName = deUser.LastName;
                        entity.Email = deUser.Email;
                        entity.UserType = "Application";
                        entity.Status = 1;
                        entity.Created_At = DateTime.Now;
                        entity.Country = string.Empty;

                        tblMgr.UpdateUserById(entity);
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Username (" + deUser.UserName + ") already exist. Please choose another username." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = "Error", Message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["userid"] = null;
            Session["username"] = null;
            Session["type"] = null;
            Session.Abandon();
            Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        private void CreatePosterDirectory()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters")))
                {
                    Directory.CreateDirectory(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"));
                }

                if (!Directory.Exists(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Images")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Images"));
                }

                if (!Directory.Exists(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Thumbnails")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters"), "Thumbnails"));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create the poster directories. Message=" + ex.Message);
            }
        }

        private DateTime ParseTwitterDateTime(string date)
        {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }

        private List<NewsEntity> ParseNewsItems(string xml, string type)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                List<NewsEntity> newsList = new List<NewsEntity>();

                // Need mechanism to track the last news publish date. Currently it will add news instead of update each time. 
                switch (type)
                {
                    case "BollywoodNewsWorld":
                        #region BollywoodNewsWorld
                        var nodes = doc.SelectNodes("rss/channel/item");
                        foreach (XmlNode node in nodes)
                        {
                            NewsEntity news = new NewsEntity();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : node.SelectSingleNode("description").InnerText;
                            news.FutureJson = string.Empty;
                            news.Image = string.Empty;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            newsList.Add(news);
                        }
                        #endregion
                        break;
                    case "GlamSham":
                        #region GlamSham
                        var items1 = doc.SelectNodes("rss/channel/item");
                        foreach (XmlNode node in items1)
                        {
                            NewsEntity news = new NewsEntity();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : node.SelectSingleNode("description").InnerText;
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("img_scoop") == null ? string.Empty : node.SelectSingleNode("img_scoop").InnerText;
                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            newsList.Add(news);
                        }
                        #endregion
                        break;
                    case "BollywoodHungama":
                        #region BollywoodHungama
                        var items2 = doc.SelectNodes("rss/channel/item");
                        foreach (XmlNode node in items2)
                        {
                            NewsEntity news = new NewsEntity();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : node.SelectSingleNode("description").InnerText;
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("image") == null ? string.Empty : node.SelectSingleNode("image").InnerText;
                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            newsList.Add(news);
                        }
                        #endregion
                        break;
                    case "HindustanTimes":
                        #region BollywoodHungama
                        var items3 = doc.SelectNodes("rss/channel/item");
                        foreach (XmlNode node in items3)
                        {
                            NewsEntity news = new NewsEntity();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : node.SelectSingleNode("description").InnerText;
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("enclosure") == null ? string.Empty : node.SelectSingleNode("enclosure").Attributes["url"].Value;
                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            newsList.Add(news);
                        }
                        #endregion
                        break;
                }

                return newsList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
