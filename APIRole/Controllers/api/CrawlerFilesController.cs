
namespace CloudMovie.APIRole.API
{
    using System;
    using DataStoreLib.Storage;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Configuration;
    using Crawler;
    using System.Web.Http;
    using System.Xml;
    using System.Diagnostics;
    using LuceneSearchLibrary;
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Models;
    using Crawler.Reviews;
    using MovieCrawler;

    /// <summary>
    /// This API returns list of all the movies details from the file on type.    
    /// </summary>
    public class CrawlerFilesController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static object _object = new object();


        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            try
            {
                // get query string parameters
                string queryParameters = this.Request.RequestUri.Query;

                if (queryParameters != null)
                {
                    var tableMgr = new TableManager();
                    var qpParams = HttpUtility.ParseQueryString(queryParameters);

                    string movieInitials = string.Empty;

                    if (!string.IsNullOrEmpty(qpParams["q"]))
                    {
                        movieInitials = qpParams["q"].ToString().ToLower();
                    }

                    var movieList = new List<XMLMovieProperties>();

                    if (string.IsNullOrEmpty(movieInitials))
                    {
                        // movie initials in empty then show all the movie from latest file
                        movieList = new XMLMovieProperties().GetMovieListFromXMLFiles(true);
                        return jsonSerializer.Value.Serialize(movieList);
                    }
                    else
                    {
                        // movie initials in not empty then show matched movie from all files
                        movieList = new XMLMovieProperties().GetMovieListFromXMLFiles(false);

                        var selectedMovies = movieList.FindAll(m => m.MovieName.ToLower().Contains(movieInitials.ToLower()));
                        return jsonSerializer.Value.Serialize(selectedMovies);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return string.Empty;
        }

        [HttpPost]
        public string CreateXMLFile(XMLMovieProperties data)
        {
            if (data == null)
            {
                return "{ \"Status\" : \"Error\" }";
            }

            try
            {
                //data = HttpContext.Current.Server.UrlDecode(data);
                JavaScriptSerializer json = new JavaScriptSerializer();
                XMLMovieProperties movieProps = data;// json.Deserialize(data, typeof(XMLMovieProperties)) as XMLMovieProperties;

                if (movieProps != null)
                {
                    XMLMovieProperties crawlMovieEntity = new XMLMovieProperties();

                    crawlMovieEntity.MovieName = movieProps.MovieName;
                    crawlMovieEntity.MovieLink = movieProps.MovieLink;
                    crawlMovieEntity.Year = Convert.ToInt32(movieProps.Month.Split(new char[] { ' ' })[1]);
                    crawlMovieEntity.Month = movieProps.Month.Split(new char[] { ' ' })[0];
                    crawlMovieEntity.Reviews = movieProps.Reviews;
                    crawlMovieEntity.SantaPosterLink = string.IsNullOrEmpty(movieProps.SantaPosterLink) ? string.Empty : movieProps.SantaPosterLink;
                    crawlMovieEntity.SaavnSongLink = string.IsNullOrEmpty(movieProps.SaavnSongLink) ? string.Empty : movieProps.SaavnSongLink;
                    crawlMovieEntity.Crawl = string.IsNullOrEmpty(movieProps.Crawl) ? "false" : movieProps.Crawl;
                    string xmlFileContent = new GenerateXMLFile().CreatingFile(crawlMovieEntity);

                    if (!string.IsNullOrEmpty(crawlMovieEntity.Crawl) && crawlMovieEntity.Crawl.ToLower() == "true")
                    {
                        // Crawl the movie
                        CrawlfromXML(xmlFileContent, crawlMovieEntity.MovieName);
                    }
                }
            }
            catch (Exception ex)
            {
                return "{ \"Status\" : \"Error\" }";
            }

            return "{ \"Status\" : \"Ok\" }";
        }

        private void CrawlfromXML(string xmlData, string movieName)
        {
            if (string.IsNullOrEmpty(xmlData)) return;

            Crawler.MovieCrawler movieCrawler = new Crawler.MovieCrawler();
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                XmlDocument xdoc = new XmlDocument();

                #region Movie Crawler
                xdoc.LoadXml(xmlData);
                var movies = xdoc.SelectNodes("Movies/Month/Movie");
                if (movies == null) return;

                foreach (XmlNode movie in movies)
                {
                    // Check movie name, we just need to crawl single movie and not all the movies present in XML file for current month
                    if (movie.Attributes["name"].Value.ToLower() != movieName.ToLower())
                    {
                        continue;
                    }

                    if (movie.Attributes["link"] != null && !string.IsNullOrEmpty(movie.Attributes["link"].Value))
                    {
                        try
                        {
                            List<string> critics = new List<string>();
                            #region Crawl Movie
                            MovieEntity mov = movieCrawler.Crawl(movie.Attributes["link"].Value);
                            TableManager tblMgr = new TableManager();
                            // Save the crawled content because in case of new movies, it fails
                            tblMgr.UpdateMovieById(mov);

                            string posterUrl = string.Empty;

                            if (movie.Attributes["santaposterlink"] != null && !string.IsNullOrEmpty(movie.Attributes["santaposterlink"].Value))
                            {
                                XMLMovieProperties prop = new XMLMovieProperties();
                                prop.SantaPosterLink = movie.Attributes["santaposterlink"].Value;
                                prop.MovieName = mov.UniqueName;

                                CrawlPosters(json.Serialize(prop));
                            }

                            // Crawl Songs from Saavn

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

                                List<ReviewEntity> reviewList = tblMgr.GetReviewByMovieId(mov.MovieId);

                                foreach (XmlNode review in reviews)
                                {
                                    ReviewEntity duplicateRE = reviewList.Find(r => r.Affiliation == review.Attributes["name"].Value);
                                    if (duplicateRE != null)
                                    {
                                        // We found the duplicate, skip this review to crawl
                                        continue;
                                    }

                                    ReviewEntity re = new ReviewEntity();
                                    string reviewLink = review.Attributes["link"].Value;

                                    switch (review.Attributes["name"].Value.Trim())
                                    {
                                        case "BollywoodHungama":
                                        case "Bollywood Hungama":
                                            re = bh.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Hindustan Times":
                                            re = ht.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "Filmfare":
                                            re = ff.Crawl(reviewLink, review.Attributes["name"].Value);
                                            break;
                                        case "CNN IBN":
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
                                        case "Mid Day":
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

                                    if (re == null)
                                        continue;

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
                            List<APIRole.UDT.Cast> casts = json.Deserialize(mov.Cast, typeof(List<APIRole.UDT.Cast>)) as List<APIRole.UDT.Cast>;
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

        private void CrawlPosters(string data)
        {
            if (string.IsNullOrEmpty(data)) return;

            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                data = HttpContext.Current.Server.UrlDecode(data);
            }
            catch (Exception)
            {
                // in some cases data is already decoded - hence we dont need to redecoded it. it throws an exception
            }

            XMLMovieProperties prop = json.Deserialize<XMLMovieProperties>(data);

            //string movieUrl,string movieUniqueName
            TableManager tblMgr = new TableManager();
            List<string> urls = new SantaImageCrawler().GetMoviePosterUrls(prop.SantaPosterLink);
            ImdbCrawler ic = new ImdbCrawler();

            MovieEntity me = tblMgr.GetMovieByUniqueName(prop.MovieName);
            List<string> processedUrl = json.Deserialize<List<string>>(me.Posters);

            me.Pictures = (string.IsNullOrEmpty(me.Pictures) || me.Pictures == "null") ? "[]" : me.Pictures;
            
            List<CloudMovie.APIRole.UDT.PosterInfo> posters = json.Deserialize<List<CloudMovie.APIRole.UDT.PosterInfo>>(me.Pictures);

            int imageCounter = 1;
            string newImageName = string.Empty;

            if (processedUrl != null)
            {
                imageCounter = processedUrl.Count + 1;

                foreach (string process in processedUrl)
                {
                    CloudMovie.APIRole.UDT.PosterInfo info = new CloudMovie.APIRole.UDT.PosterInfo();
                    info.url = process;
                    posters.Add(info);
                }
            }
            else
            {
                processedUrl = new List<string>();
                posters = new List<CloudMovie.APIRole.UDT.PosterInfo>();

            }

            foreach (string url in urls)
            {
                CloudMovie.APIRole.UDT.PosterInfo info = new CloudMovie.APIRole.UDT.PosterInfo();

                try
                {
                    string posterPath = ic.GetNewImageName(prop.MovieName, ic.GetFileExtension(url), imageCounter, false, ref newImageName);
                    ic.DownloadImage(url, posterPath);

                    processedUrl.Add(newImageName);

                    info.url = newImageName;
                    info.source = prop.SantaPosterLink;
                    posters.Add(info);

                    imageCounter++;
                }
                catch (Exception)
                {
                    // Skip that image
                }
            }

            me.Posters = json.Serialize(processedUrl);
            me.Pictures = json.Serialize(posters);
            tblMgr.UpdateMovieById(me);
        }
    }
}
