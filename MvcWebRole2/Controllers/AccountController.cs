using Crawler;
using Crawler.Reviews;
using DataStoreLib.BlobStorage;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using LuceneSearchLibrarby;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        static IDictionary<string, MovieEntity> allMovies = null;
        static List<string> allBlobUrls = null;
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


                //string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                string basePath = @"D:\GitHub-SVN\moviemirchi\MvcWebRole2\App_Data\";
                string[] moviesFilePath = Directory.GetFiles(basePath, "*.xml");

                #region Movie Crawler
                foreach (string filePath in moviesFilePath)
                {
                    xdoc.Load(filePath);

                    //var movies = xdoc.SelectNodes("Movies/Month[@name='December']/Movie");
                    var movies = xdoc.SelectNodes("Movies/Month/Movie");

                    if (movies == null)
                        continue;

                    foreach (XmlNode movie in movies)
                    {
                        if (movie.Attributes["link"] != null && !string.IsNullOrEmpty(movie.Attributes["link"].Value))
                        {
                            try
                            {
                                List<string> critics = new List<string>();
                                #region Crawl Movie
                                MovieEntity mov = movieCrawler.Crawl(movie.Attributes["link"].Value);
                                TableManager tblMgr = new TableManager();
                                string posterUrl = string.Empty;

                                if (string.IsNullOrEmpty(mov.RowKey) || string.IsNullOrEmpty(mov.MovieId)) continue;

                                tblMgr.UpdateMovieById(mov);
                                #endregion

                                #region Crawl Movie Reviews
                                #region Crawler
                                try
                                {
                                    #region Reviews from different sites
                                    BollywoodHungamaReviews bh = new BollywoodHungamaReviews();
                                    HindustanTimesReviews ht = new HindustanTimesReviews();
                                    FilmfareReviews ff = new FilmfareReviews();
                                    CnnIbn cibn = new CnnIbn();
                                    BoxOfficeIndia boi = new BoxOfficeIndia();
                                    Dna dna = new Dna();
                                    FirstPost fp = new FirstPost();
                                    IndianExpress ie = new IndianExpress();
                                    KomalNahta kn = new KomalNahta();
                                    MidDay md = new MidDay();
                                    Ndtv ndtv = new Ndtv();
                                    Rajasen rs = new Rajasen();
                                    Rediff rdf = new Rediff();
                                    Telegraph tg = new Telegraph();
                                    TheHindu th = new TheHindu();
                                    TimesOfIndia toi = new TimesOfIndia();
                                    AnupamaChopra ac = new AnupamaChopra();
                                    MumbaiMirror mm = new MumbaiMirror();
                                    #endregion

                                    var reviews = movie.SelectNodes("Review");
                                    foreach (XmlNode review in reviews)
                                    {
                                        ReviewEntity re = null;
                                        string reviewLink = review.Attributes["link"].Value;

                                        if (!string.IsNullOrEmpty(reviewLink))
                                        {
                                            switch (review.Attributes["name"].Value.Trim())
                                            {
                                                case "BollywoodHungama":
                                                    re = bh.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Hindustan Times":
                                                    re = ht.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Filmfare":
                                                    re = ff.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "CNNIBN":
                                                    re = cibn.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Box Office India":
                                                    re = boi.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "DNA":
                                                    re = dna.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "FirstPost":
                                                    re = fp.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Indian Express":
                                                    re = ie.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Komal Nahta's Blog":
                                                    re = kn.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "MidDay":
                                                    re = md.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "NDTV":
                                                    re = ndtv.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "rajasen.com":
                                                    re = rs.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Rediff":
                                                    re = rdf.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Telegraph":
                                                    re = tg.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "The Hindu":
                                                    re = th.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Times of India":
                                                    re = toi.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "anupamachopra.com":
                                                    re = ac.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                                case "Mumbai Mirror":
                                                    re = mm.Crawl(reviewLink, review.Attributes["name"].Value);
                                                    break;
                                            }
                                        }

                                        if (re != null)
                                        {
                                            critics.Add(re.ReviewerName);
                                            // update the IDs - Movie Id, Reviewer Id etc.
                                            string reviewerId = ReviewCrawler.SetReviewer(re.ReviewerName, review.Attributes["name"].Value);
                                            //re.RowKey = re.ReviewId = new Guid().ToString();
                                            re.ReviewerId = reviewerId;
                                            re.MovieId = mov.MovieId;
                                            re.OutLink = reviewLink;

                                            var reviewsEntity = tblMgr.GetReviewByMovieId(mov.MovieId);

                                            if (!string.IsNullOrEmpty(reviewerId) && !string.IsNullOrEmpty(re.ReviewId) && !string.IsNullOrEmpty(re.MovieId) &&
                                                reviewsEntity.Find(r => r.ReviewerName.ToLower().Trim() == re.ReviewerName.ToLower().Trim()) == null)
                                            {
                                                tblMgr.UpdateReviewById(re);
                                            }
                                        }
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
                                        // actor, director, music, producer
                                        string role = actor.role.ToLower();
                                        string characterName = string.IsNullOrEmpty(actor.charactername) ? string.Empty : actor.charactername;

                                        // Check if artist is already present in the list for some other role.
                                        // If yes, skip it. Also if the actor name is missing then skip the artist
                                        if (actors.Contains(actor.name) || string.IsNullOrEmpty(actor.name) || actor.name == "null")
                                            continue;

                                        // If we want to showcase main artists and not all, keep the following switch... case.
                                        switch (role)
                                        {
                                            case "actor":
                                                actors.Add(actor.name);
                                                break;
                                            case "producer":
                                                // some times producer are listed as line producer etc. 
                                                // We are not interested in those artists as of now?! Hence skipping it
                                                if (characterName == role)
                                                {
                                                    actors.Add(actor.name);
                                                }
                                                break;
                                            case "music":
                                            case "director":
                                                // Main music director and movie director does not have associated character name.
                                                // Where as other side directors have associated character name as associate director, assitant director.
                                                // Skipping such cases.
                                                if (string.IsNullOrEmpty(characterName))
                                                {
                                                    actors.Add(actor.name);
                                                }
                                                break;
                                        }

                                        // If we want to showcase all the technicians 
                                        //actors.Add(actor.name);
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
                                movieSearchIndex.Critics = json.Serialize(critics);
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
        public void GetTweets(string blobXmlFilePath = "")
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
                    string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
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

        [HttpGet]
        public void GetNews(string blobXmlFilePath = "")
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                string newsXml = string.Empty;

                if (blobXmlFilePath == "")
                {
                    string basePath = Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
                    string filePath = Path.Combine(basePath, "News.xml");
                    xdoc.Load(filePath);
                }
                else
                {
                    xdoc.Load(blobXmlFilePath);
                }

                var items = xdoc.SelectNodes("News/Link");
                if (items != null)
                {
                    foreach (XmlNode item in items)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.InnerText);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            #region Get News Content
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;
                            if (response.CharacterSet == null)
                                readStream = new StreamReader(receiveStream);
                            else
                                readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                            newsXml = readStream.ReadToEnd();
                            List<NewsEntity> news = ParseNewsItems(newsXml, item.Attributes["type"].Value);
                            TableManager tblMgr = new TableManager();
                            tblMgr.UpdateNewsById(news);
                            response.Close();
                            readStream.Close();
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [HttpGet]
        public void GetArtists()
        {
            try
            {
                TableManager tblMgr = new TableManager();
                Crawler.ArtistCrawler artistCrawler = new Crawler.ArtistCrawler();

                var movies = tblMgr.GetAllMovies();

                foreach (MovieEntity movie in movies.Values)
                {
                    try
                    {
                        var items = JsonConvert.DeserializeObject(movie.Casts);
                        JArray array = JArray.Parse(movie.Casts);
                        List<Cast> castList = new List<Cast>();

                        foreach (JObject o in array.Children<JObject>())
                        {
                            Cast cast = new Cast();

                            foreach (JProperty p in o.Properties())
                            {
                                switch (p.Name)
                                {
                                    case "charactername":
                                        cast.charactername = p.Value.ToString();
                                        break;
                                    case "link":
                                        int index = p.Value.ToString().IndexOf("?");
                                        string linkPath = (index < 0) ? p.Value.ToString() : p.Value.ToString().Remove(index);
                                        cast.link = "http://www.imdb.com" + linkPath;
                                        break;
                                    case "name":
                                        cast.name = p.Value.ToString();
                                        break;
                                    case "role":
                                        cast.role = p.Value.ToString();
                                        break;
                                }
                            }

                            if (new TableManager().GetArtist(cast.name) == null && castList.Find(c => c.name == cast.name) == null)
                                castList.Add(cast);
                        }

                        artistCrawler.CrawlArtists(castList);

                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        [HttpGet]
        public void GetSongs()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            SetConnectionString();

            try
            {
                string file = @"D:\GitHub-SVN\moviemirchi\MvcWebRole2\App_Data\Movie-Songs-2014.xml";
                #region Crawl Movie
                List<MovieSongsProps> movieSongs = new GenerateXMLFile().GetMoviesSongsProps(file);

                TableManager tbleMgr = new TableManager();

                if (movieSongs != null)
                {
                    foreach (MovieSongsProps movieSong in movieSongs)
                    {
                        if (!string.IsNullOrEmpty(movieSong.MovieName) && !string.IsNullOrEmpty(movieSong.MovieSongLink))
                        {
                            List<Songs> songs = new SongCrawler().Crawl(movieSong.MovieSongLink);

                            if (songs == null) continue;

                            movieSong.MovieName = movieSong.MovieName.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                            MovieEntity movie = tbleMgr.GetMovieByUniqueName(movieSong.MovieName);

                            if (movie != null)
                            {
                                string strSongs = json.Serialize(songs);
                                movie.Songs = strSongs;

                                tbleMgr.UpdateMovieById(movie);
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error occured while crawing songs, error: {0}", ex.Message));
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
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : Util.StripHTMLTags(node.SelectSingleNode("description").InnerText);
                            news.FutureJson = string.Empty;
                            news.Image = string.Empty;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            news.MovieName = string.Empty;
                            news.ArtistName = string.Empty;
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
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : Util.StripHTMLTags(node.SelectSingleNode("description").InnerText);
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("img_scoop") == null ? string.Empty : node.SelectSingleNode("img_scoop").InnerText;

                            //download image in our blob storage
                            if (!string.IsNullOrEmpty(news.Image))
                            {
                                news.Image = Util.DownloadImage(news.Image, news.NewsId);
                            }

                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            news.MovieName = string.Empty;
                            news.ArtistName = string.Empty;
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
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : Util.StripHTMLTags(node.SelectSingleNode("description").InnerText);
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("image") == null ? string.Empty : node.SelectSingleNode("image").InnerText;

                            //download image in our blob storage
                            if (!string.IsNullOrEmpty(news.Image))
                            {
                                news.Image = Util.DownloadImage(news.Image, news.NewsId);
                            }

                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            news.MovieName = string.Empty;
                            news.ArtistName = string.Empty;
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
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Description = node.SelectSingleNode("description") == null ? string.Empty : Util.StripHTMLTags(node.SelectSingleNode("description").InnerText);
                            news.FutureJson = string.Empty;
                            news.Image = node.SelectSingleNode("enclosure") == null ? string.Empty : node.SelectSingleNode("enclosure").Attributes["url"].Value;

                            //download image in our blob storage
                            if (!string.IsNullOrEmpty(news.Image))
                            {
                                news.Image = Util.DownloadImage(news.Image, news.NewsId);
                            }

                            news.PublishDate = node.SelectSingleNode("pubDate") == null ? string.Empty : node.SelectSingleNode("pubDate").InnerText;
                            news.RowKey = news.NewsId = Guid.NewGuid().ToString();
                            news.Source = type;
                            news.Link = node.SelectSingleNode("link") == null ? string.Empty : node.SelectSingleNode("link").InnerText;
                            news.Title = node.SelectSingleNode("title") == null ? string.Empty : node.SelectSingleNode("title").InnerText;
                            news.MovieName = string.Empty;
                            news.ArtistName = string.Empty;
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

        public void CrawlfromXML(string xmlData)
        {
            if (string.IsNullOrEmpty(xmlData)) return;

            Crawler.MovieCrawler movieCrawler = new Crawler.MovieCrawler();
            JavaScriptSerializer json = new JavaScriptSerializer();

            SetConnectionString();
            //CreatePosterDirectory();

            try
            {
                XmlDocument xdoc = new XmlDocument();

                #region Movie Crawler
                xdoc.LoadXml(xmlData);
                var movies = xdoc.SelectNodes("Movies/Month/Movie");
                if (movies == null) return;

                foreach (XmlNode movie in movies)
                {
                    if (movie.Attributes["link"] != null && !string.IsNullOrEmpty(movie.Attributes["link"].Value))
                    {
                        try
                        {
                            List<string> critics = new List<string>();
                            #region Crawl Movie
                            MovieEntity mov = movieCrawler.Crawl(movie.Attributes["link"].Value);
                            TableManager tblMgr = new TableManager();
                            string posterUrl = string.Empty;

                            if (string.IsNullOrEmpty(mov.RowKey) || string.IsNullOrEmpty(mov.MovieId)) continue;

                            tblMgr.UpdateMovieById(mov);
                            #endregion

                            #region Crawl Movie Reviews
                            #region Crawler
                            try
                            {
                                BollywoodHungamaReviews bh = new BollywoodHungamaReviews();
                                HindustanTimesReviews ht = new HindustanTimesReviews();
                                FilmfareReviews ff = new FilmfareReviews();
                                CnnIbn cibn = new CnnIbn();
                                BoxOfficeIndia boi = new BoxOfficeIndia();
                                Dna dna = new Dna();
                                FirstPost fp = new FirstPost();
                                IndianExpress ie = new IndianExpress();
                                KomalNahta kn = new KomalNahta();
                                MidDay md = new MidDay();
                                Ndtv ndtv = new Ndtv();
                                Rajasen rs = new Rajasen();
                                Rediff rdf = new Rediff();
                                Telegraph tg = new Telegraph();
                                TheHindu th = new TheHindu();
                                TimesOfIndia toi = new TimesOfIndia();
                                AnupamaChopra ac = new AnupamaChopra();
                                MumbaiMirror mm = new MumbaiMirror();

                                var reviews = movie.SelectNodes("Review");
                                foreach (XmlNode review in reviews)
                                {
                                    ReviewEntity re = new ReviewEntity();
                                    string reviewLink = review.Attributes["link"].Value;

                                    switch (review.Attributes["name"].Value.Trim())
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
                                        case "CNNIBN":
                                            re = cibn.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Box Office India":
                                            re = boi.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "DNA":
                                            re = dna.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "FirstPost":
                                            re = fp.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Indian Express":
                                            re = ie.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Komal Nahta's Blog":
                                            re = kn.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "MidDay":
                                            re = md.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "NDTV":
                                            re = ndtv.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "rajasen.com":
                                            re = rs.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Rediff":
                                            re = rdf.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Telegraph":
                                            re = tg.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "The Hindu":
                                            re = fp.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Times of India":
                                            re = toi.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "anupamachopra.com":
                                            re = ac.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Mumbai Mirror":
                                            re = mm.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                    }

                                    critics.Add(re.ReviewerName);

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
                                    // actor, director, music, producer
                                    string role = actor.role.ToLower();
                                    string characterName = string.IsNullOrEmpty(actor.charactername) ? string.Empty : actor.charactername;

                                    // Check if artist is already present in the list for some other role.
                                    // If yes, skip it. Also if the actor name is missing then skip the artist
                                    if (actors.Contains(actor.name) || string.IsNullOrEmpty(actor.name) || actor.name == "null")
                                        continue;

                                    // If we want to showcase main artists and not all, keep the following switch... case.
                                    switch (role)
                                    {
                                        case "actor":
                                            actors.Add(actor.name);
                                            break;
                                        case "producer":
                                            // some times producer are listed as line producer etc. 
                                            // We are not interested in those artists as of now?! Hence skipping it
                                            if (characterName == role)
                                            {
                                                actors.Add(actor.name);
                                            }
                                            break;
                                        case "music":
                                        case "director":
                                            // Main music director and movie director does not have associated character name.
                                            // Where as other side directors have associated character name as associate director, assitant director.
                                            // Skipping such cases.
                                            if (string.IsNullOrEmpty(characterName))
                                            {
                                                actors.Add(actor.name);
                                            }
                                            break;
                                    }

                                    // If we want to showcase all the technicians 
                                    //actors.Add(actor.name);
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
                            movieSearchIndex.Critics = json.Serialize(critics);
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

                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: {0}", ex);
                throw;
            }

        }

        public void RebuildPosters()
        {
            TableManager tblMgr = new TableManager();
            BlobStorageService blobMgr = new BlobStorageService();

            try
            {
                IDictionary<string, MovieEntity> movies = null;
                List<string> blobUrls = null;
                List<string> finalUrl = new List<string>();
                if (allBlobUrls == null)
                {
                    blobUrls = blobMgr.GetUploadedFileFromBlob(BlobStorageService.Blob_ImageContainer);

                    foreach (string url in blobUrls)
                    {
                        if (url.Contains("-thumb-"))
                            continue;

                        string tempUrl = url.Substring(url.LastIndexOf("/") + 1);
                        finalUrl.Add(tempUrl);
                    }

                    allBlobUrls = finalUrl;
                    blobUrls = allBlobUrls;
                }
                else
                    blobUrls = allBlobUrls;

                if (allMovies == null)
                {
                    movies = tblMgr.GetAllMovies();
                    allMovies = movies;
                }
                else
                    movies = allMovies;

                foreach (MovieEntity movie in movies.Values)
                {
                    try
                    {
                        string pattern = movie.UniqueName + "-poster-";
                        List<string> posters = blobMgr.GetAllFiles(blobUrls, pattern);

                        if (posters != null && posters.Count > 0)
                        {
                            JavaScriptSerializer json = new JavaScriptSerializer();
                            string posterJson = json.Serialize(posters);
                            movie.Posters = posterJson;
                            tblMgr.UpdateMovieById(movie);

                            #region Lucene Index
                            LuceneSearch.ClearLuceneIndexRecord(movie.MovieId, "Id");
                            LuceneSearch.ClearLuceneIndexRecord(movie.UniqueName, "UniqueName");

                            string posterUrl = "default-movie.jpg";
                            string critics = string.Empty;

                            if (!string.IsNullOrEmpty(movie.Posters))
                            {
                                List<string> pList = json.Deserialize(movie.Posters, typeof(List<string>)) as List<string>;
                                if (pList != null && pList.Count > 0)
                                    posterUrl = pList[pList.Count - 1];
                            }

                            var reviewDic = tblMgr.GetReviewsByMovieId(movie.MovieId);

                            if (reviewDic != null && reviewDic.Values != null && reviewDic.Values.Count > 0)
                            {
                                List<string> lCritics = new List<string>();

                                foreach (ReviewEntity re in reviewDic.Values)
                                {
                                    lCritics.Add(re.ReviewerName);
                                }

                                critics = json.Serialize(lCritics);
                            }

                            MovieSearchData movieSearchIndex = new MovieSearchData();
                            movieSearchIndex.Id = movie.RowKey;
                            movieSearchIndex.Title = movie.Name;
                            movieSearchIndex.Type = movie.Genre;
                            movieSearchIndex.TitleImageURL = posterUrl;
                            movieSearchIndex.UniqueName = movie.UniqueName;
                            movieSearchIndex.Description = movie.Casts;
                            movieSearchIndex.Critics = critics;
                            movieSearchIndex.Link = movie.UniqueName;
                            LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
                            #endregion
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Failed Movie:" + movie.UniqueName);
                    }
                }

                //tblMgr.UpdateMoviesById(movies.Values);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RebuildLucene()
        {

        }

        [HttpGet]
        public void FilterMovies()
        {
            TableManager tblMgr = new TableManager();
            Crawler.ArtistCrawler artistCrawler = new Crawler.ArtistCrawler();

            var movies = tblMgr.GetAllMovies();

            //var unleased2014Movie = movies.FindAll(m => m.Year.Trim() == "2014" && m.State.Trim() != "upcoming");

            foreach (MovieEntity movie in movies.Values)
            {
                if (movie.Year.Trim() == "2014" && movie.State.Trim() != "upcoming")
                {
                    var items = JsonConvert.DeserializeObject(movie.Casts);
                    JArray array = JArray.Parse(movie.Casts);
                    List<Cast> castList = new List<Cast>();

                    bool hasLink = false;

                    foreach (JObject o in array.Children<JObject>())
                    {
                        Cast cast = new Cast();

                        foreach (JProperty p in o.Properties())
                        {
                            switch (p.Name)
                            {
                                case "charactername":
                                    cast.charactername = p.Value.ToString();
                                    break;
                                case "link":
                                    hasLink = true;
                                    int index = p.Value.ToString().IndexOf("?");
                                    string linkPath = (index < 0) ? p.Value.ToString() : p.Value.ToString().Remove(index);
                                    cast.link = "http://www.imdb.com" + linkPath;
                                    break;
                                case "name":
                                    cast.name = p.Value.ToString();
                                    break;
                                case "role":
                                    cast.role = p.Value.ToString();
                                    break;
                            }
                        }
                    }

                    if (!hasLink)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\GitHub-SVN\moviemirchi\Temp-Logs\2014-UnReleased-Moives.txt", true))
                        {
                            file.WriteLine(string.Format("Movie: {0} || Month: {1} || Year: {2}", movie.Name, movie.Month, movie.Year));
                        }
                    }
                }
            }
        }
    }
}
