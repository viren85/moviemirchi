
namespace MovieCrawler
{
    using Crawler;
    using DataStoreLib.BlobStorage;
    using DataStoreLib.Models;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ImdbCrawler
    {
        /*private static readonly string PosterPath = Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters");
        private static readonly string PosterImagePath = Path.Combine(PosterPath, "Images");
        private static readonly string PosterThumbnailPath = Path.Combine(PosterPath, "Thumbnails");*/
        
        private CrawlerHelper helper = new CrawlerHelper();

        // Parent Node/Body
        public MovieEntity GetMovieDetails(HtmlNode body)
        {
            MovieEntity movie = new MovieEntity();
            movie.RowKey = movie.MovieId = Guid.NewGuid().ToString();
            movie.Name = GetMovieName(body);
            movie.AltNames = GetMovieByAltName(body);
            movie.UniqueName = GetMovieUniqueName(movie.Name);
            movie.Ratings = GetMovieRating(body);
            movie.Synopsis = GetMovieStory(body);
            movie.Genre = GetMovieGenre(body);
            movie.Month = GetMovieMonth(body);
            movie.Year = GetMovieYear(body);
            movie.Stats = GetMovieStats(body);

            movie.Trailers = string.Empty;
            movie.Pictures = string.Empty;
            movie.State = string.Empty;
            movie.MyScore = Util.DEFAULT_SCORE;
            movie.JsonString = "[]";

            movie.Name = movie.Name.Replace(":", string.Empty);
            movie.AltNames = movie.AltNames.Replace(":", string.Empty);
            movie.UniqueName = movie.UniqueName.Replace(":", string.Empty);

            return movie;
        }

        // Movie Name
        private string GetMovieName(HtmlNode body)
        {
            var headerNode = helper.GetElementWithAttribute(body, "h1", "class", "header");
            var movieName = helper.GetElementWithAttribute(headerNode, "span", "class", "itemprop");
            return movieName.InnerText;
        }

        //Movie Alt Name
        private string GetMovieByAltName(HtmlNode body)
        {
            var headerNode = helper.GetElementWithAttribute(body, "h1", "class", "header");
            var movieName = helper.GetElementWithAttribute(headerNode, "span", "class", "itemprop");
            return movieName.InnerText;
        }

        // Movie Year
        private string GetMovieYear(HtmlNode body)
        {
            var headerNode = helper.GetElementWithAttribute(body, "h1", "class", "header");
            var yearLink = helper.GetElementWithAttribute(headerNode, "span", "class", "nobr");
            return yearLink != null ? yearLink.InnerText.Replace("(", string.Empty).Replace(")", string.Empty) : string.Empty;
        }

        // Rating
        private string GetMovieRating(HtmlNode body)
        {
            var ratingNode = helper.GetElementWithAttribute(body, "div", "class", "star-box");
            var rate = helper.GetElementWithAttribute(body, "div", "class", "star-box-giga-star");
            return rate != null ? rate.InnerText : string.Empty;
        }

        // Genre
        private string GetMovieGenre(HtmlNode body)
        {
            var genreNode = helper.GetElementWithAttribute(body, "div", "class", "infobar");
            var genres = genreNode.Elements("a");

            string genre = genres != null ?
                    String.Join(" | ", genres.Select(node => node.InnerHtml))
                    : String.Empty;
            return genre;
        }

        // Story or Synopsis
        private string GetMovieStory(HtmlNode body)
        {
            try
            {
                var storyNode = helper.GetElementWithAttribute(body, "div", "class", "canwrap");
                var writerNode = helper.GetElementWithAttribute(storyNode, "em", "class", "nobr");
                var parent = storyNode.Element("p");

                if (writerNode != null)
                    storyNode = parent.RemoveChild(writerNode);

                return parent != null ? parent.InnerText : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //Month
        private string GetMovieMonth(HtmlNode body)
        {
            var genreNode = helper.GetElementWithAttribute(body, "div", "class", "infobar");
            var month = helper.GetElementWithAttribute(genreNode, "span", "class", "nobr");
            var releaseDate = month.Element("a");

            return releaseDate != null ? releaseDate.InnerText : string.Empty;
        }

        // Plot Summary
        private string GetPlotSummary(HtmlNode body)
        {
            return string.Empty;
        }

        // Cast/Crew
        // return json string of cast which includes director, producer, music director, stars, writer, editor etc.
        public string GetMovieCast(HtmlNode body)
        {
            /*
             var director = helper.GetElementWithAttribute(body, "div", "class", "txt-block");
             var writer = helper.GetElementWithAttribute(body, "div", "itemprop", "creator"); 
             var actor = helper.GetElementWithAttribute(body, "div", "itemprop", "actors"); 
             */

            var loadlate = helper.GetElementWithAttribute(body, "table", "class", "cast_list");

            HtmlDocument htmlDoc = new HtmlDocument();
            //var space = htmlDoc.castNode.SelectNodes();

            //store href, src, height, width, name of image in innerhtml
            var fullcast = helper.GetElementWithAttribute(body, "table", "class", "cast_list");
            //var castImg = helper.GetElementWithAttribute(fullcast, "td", "class", string.Empty);
            /*
            var characterName = helper.GetElements(body, "table", "class", "cast_list");


            var cast = fullcast.InnerText;


            cast.Trim();
            string replacement = Regex.Replace(cast, @"\n|\v|\\s", string.Empty);



            replacement.Substring(0, 3);

            */
            return fullcast != null ? fullcast.GetAttributeValue("altName", string.Empty) : string.Empty;
        }

        public string GetMovieStats(HtmlNode body)
        {
            var statsNode = helper.GetElementWithAttribute(body, "div", "id", "titleDetails");
            var statsContainer = statsNode.Elements("h3");

            if (statsContainer != null)
            {
                HtmlNode boxOfficeNode = statsContainer.FirstOrDefault(node => 0 == string.Compare(node.InnerHtml, "box office", true));

                if (boxOfficeNode != default(HtmlNode))
                {
                    var opening = boxOfficeNode.NextSibling.NextSibling;
                    var gross = opening.NextSibling.NextSibling;
                    return opening.InnerText.Trim() + " | " + gross.InnerText.Trim();
                }
            }

            return string.Empty;
        }

        // Pricing/Business Information
        public string GetMovieBusinessDetails(HtmlNode body)
        {
            // return the revenue generated by this movie
            return string.Empty;
        }

        // Posters
        //get the poster link
        public string GetMoviePoster(HtmlNode body)
        {
            var posterNode = helper.GetElementWithAttribute(body, "table", "id", "title-overview-widget-layout");
            var posterLink = helper.GetElementWithAttribute(posterNode, "img", "itemprop", "image");
            return posterLink.Attributes["src"].Value;
        }

        public List<Songs> GetSongDetails(HtmlNode body)
        {
            var listNode = helper.GetElementWithAttribute(body, "div", "id", "soundtracks_content");

            List<Songs> songs = new List<Songs>();
            if (listNode != null)
            {
                var nodes = listNode.Elements("div");
                foreach (HtmlNode node in nodes)
                {
                    if (node.Attributes["class"] != null && node.Attributes["class"].Value == "list")
                    {
                        var songItems = node.Elements("div");

                        if (songItems != null)
                        {
                            foreach (HtmlNode song in songItems)
                            {
                                Songs songDetail = new Songs();
                                string title, lyrics, composer, performer, recite, courtsey;

                                GetSongDetails(song, out title, out lyrics, out composer, out performer, out recite, out courtsey);
                                songDetail.SongTitle = Cleanse(title);
                                songDetail.Lyrics = Cleanse(lyrics);
                                songDetail.Composed = Cleanse(composer);
                                songDetail.Performer = Cleanse(performer);
                                songDetail.Recite = Cleanse(recite);
                                songDetail.Courtsey = Cleanse(courtsey);

                                if (!title.Contains("It looks like we don't have any Soundtracks for this title yet."))
                                {
                                    songs.Add(songDetail);
                                }
                            }
                        }
                    }
                }
            }

            return songs;
        }

        private static string Cleanse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Replace(":", string.Empty).Replace(",", string.Empty).Trim();
        }

        private bool GetSongDetails(HtmlNode song, out string title, out string lyrics, out string composer, out string performer, out string recite, out string courtsey)
        {
            HtmlNodeCollection songs = song.ChildNodes;
            string currentRole = string.Empty;
            string lastRole = string.Empty;
            string name = string.Empty;
            bool hasName = false;
            title = lyrics = composer = performer = recite = courtsey = string.Empty;

            for (int count = 0; count < songs.Count; count++)
            {
                try
                {
                    // Get the song title. It will be always on first position
                    if (count == 0)
                    {
                        title = songs[count].InnerText;
                    }
                    else
                    {
                        switch (songs[count].Name)
                        {
                            case "br":
                                break;
                            case "#text":
                                currentRole = GetSongDetails(songs[count].InnerText, ref lastRole, ref hasName, ref name);
                                if (hasName)
                                {
                                    switch (currentRole)
                                    {
                                        case "Lyrics":
                                            lyrics += name + ", ";
                                            break;
                                        case "Composer":
                                            composer += name + ", ";
                                            break;
                                        case "Performer":
                                            performer += name + ", ";
                                            break;
                                        case "Courtesy":
                                            courtsey += name + ", ";
                                            break;
                                        case "Recite":
                                            recite += name + ", ";
                                            break;
                                    }

                                    hasName = false;
                                }
                                break;
                            case "a":
                                switch (currentRole)
                                {
                                    case "Lyrics":
                                        lyrics += songs[count].InnerText + ", ";
                                        break;
                                    case "Composer":
                                        composer += songs[count].InnerText + ", ";
                                        break;
                                    case "Performer":
                                        performer += songs[count].InnerText + ", ";
                                        break;
                                    case "Courtesy":
                                        courtsey += songs[count].InnerText + ", ";
                                        break;
                                    case "Recite":
                                        recite += songs[count].InnerText + ", ";
                                        break;
                                }

                                lastRole = currentRole;
                                break;
                        }

                        if (currentRole == "Courtesy" && string.IsNullOrEmpty(courtsey))
                        {
                            courtsey = songs[count].InnerText;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return true;
        }

        private string GetSongDetails(string role, ref string lastRole, ref bool hasName, ref string name)
        {
            string currentRole = string.Empty;

            if (string.IsNullOrEmpty(role))
            {
                return string.Empty;
            }

            role = role.ToLower();
            if (role.Contains("written"))
            {
                currentRole = "Lyrics";
            }
            else if (role.Contains("composed"))
            {
                currentRole = "Composer";
            }
            else if (role.Contains("performed"))
            {
                currentRole = "Performer";
            }
            else if (role.Contains("courtesy"))
            {
                currentRole = "Courtesy";
            }
            else if (role.Contains("recited"))
            {
                currentRole = "Recite";
            }
            else if (role.Contains("and") || role.Contains(","))
            {
                currentRole = lastRole;
            }

            if (role.Contains("by") && !role.Trim().EndsWith("by") && role.Length > 10)
            {
                try
                {
                    name = role.Substring(role.IndexOf("by"));
                    name = name.Replace("by", string.Empty).Trim();
                    hasName = true;
                }
                catch (Exception)
                {
                    // TODO: Ideally we should never hit
                }
            }

            return currentRole;
        }

        public string GetMovieTrailers(HtmlNode body)
        {
            return string.Empty;
        }

        public string GetMoviePictures(HtmlNode body)
        {
            return string.Empty;
        }

        public string GetMovieUniqueName(string movieName)
        {
            string uniqueName = movieName
                .Replace(" ", "-")
                .Replace("&", "-and-")
                .Replace(".", string.Empty)
                .Replace("'", string.Empty)
                .ToLower();

            /*var tableMgr = new TableManager();
            
            MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(uniqueName);

            if (oldEntity != null)
            {
                var rand = new Random();
                int num = rand.Next(999999);

                oldEntity.UniqueName = num.ToString() + oldEntity.UniqueName;

                tableMgr.UpdateMovieById(oldEntity);
            }*/

            return uniqueName;
        }

        public List<Cast> GetMovieCastDetails(HtmlNode body)
        {
            var headerNode = helper.GetElementWithAttribute(body, "div", "id", "fullcredits_content");

            List<Cast> castList = new List<Cast>();
            var moviesCast = headerNode.Elements("h4");

            if (moviesCast != null)
            {
                foreach (HtmlNode cast in moviesCast)
                {
                    string castRole = GetCleanRoleName(cast.InnerText);

                    if (castRole == "Cast")
                    {
                        PopulateCast(cast.NextSibling.NextSibling, ref castList, "Actor");
                    }
                    else
                    {
                        PopulateTeam(cast.NextSibling.NextSibling, ref castList, castRole);
                    }
                }
            }

            //var movieName = helper.GetElementWithAttribute(headerNode, "span", "class", "itemprop");
            return castList;
        }

        private void CrawlPosterImagePath(string url, string movieName, int imageCounter, ref bool isThumbnailDownloaded, ref List<string> posterImagePath, ref string thumbnailImagePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        string data = readStream.ReadToEnd();

                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.OptionFixNestedTags = true;
                        htmlDoc.LoadHtml(data);
                        if (htmlDoc.DocumentNode != null)
                        {
                            HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                            if (bodyNode == null)
                            {
                                Console.WriteLine("body node is null");
                            }
                            else
                            {
                                var thumbnailContainer = helper.GetElementWithAttribute(bodyNode, "div", "class", "media_single");
                                var posterThumbnail = helper.GetElementWithAttribute(thumbnailContainer, "img", "class", "poster");

                                var poster = helper.GetElementWithAttribute(bodyNode, "img", "id", "primary-img");
                                string newImageName = string.Empty;

                                if (posterThumbnail != null && posterThumbnail.Attributes["src"] != null && !isThumbnailDownloaded)
                                {
                                    string thumbnailPath = GetNewImageName(movieName, GetFileExtension(posterThumbnail.Attributes["src"].Value), imageCounter, true, ref newImageName);
                                    DownloadImage(posterThumbnail.Attributes["src"].Value, thumbnailPath);
                                    thumbnailImagePath = newImageName;
                                    isThumbnailDownloaded = true;
                                }

                                if (poster != null && poster.Attributes["id"] != null)
                                {
                                    string posterPath = GetNewImageName(movieName, GetFileExtension(poster.Attributes["src"].Value), imageCounter, false, ref newImageName);
                                    posterImagePath.Add(newImageName);
                                    DownloadImage(poster.Attributes["src"].Value, posterPath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Url:" + url);
                Debug.WriteLine("Message:" + ex.Message);
            }
        }

        public List<string> GetMoviePosterDetails(HtmlNode body, string movieName, ref List<string> posterPath, ref string thumbnailPath)
        {
            posterPath = posterPath ?? new List<string>();

            thumbnailPath = string.Empty;

            bool isThumbnailDownloaded = false;
            var thumbListNode = helper.GetElementWithAttribute(body, "div", "class", "media_index_thumb_list");

            if (thumbListNode != null)
            {
                var thumbnails = thumbListNode.Elements("a");
                if (thumbnails != null)
                {
                    //int imageCounter = GetMaxImageCounter(movieName);
                    int imageCounter = new BlobStorageService().GetImageFileCount(BlobStorageService.Blob_ImageContainer, movieName.Replace(" ", "-").ToLower() + "-poster-");

                    foreach (HtmlNode thumbnail in thumbnails)
                    {
                        if (thumbnail.Attributes["itemprop"] != null && thumbnail.Attributes["itemprop"].Value == "thumbnailUrl")
                        {
                            string href = thumbnail.Attributes["href"].Value;
                            CrawlPosterImagePath("http://imdb.com" + href, movieName, imageCounter, ref isThumbnailDownloaded, ref posterPath, ref thumbnailPath);
                            imageCounter++;
                        }
                    }
                }
            }

            return posterPath;
        }

        public List<string> GetBollywoodHungamaMoviePosterDetails(HtmlNode body, string movieName, ref List<string> posterPath, ref string thumbnailPath)
        {
            posterPath = posterPath ?? new List<string>();
            thumbnailPath = string.Empty;

            var aListNode = helper.GetElementWithAttribute(body, "a", "class", "imgthpic");

            if (aListNode != null)
            {
                var thumbnails = aListNode.Elements("img");
                if (thumbnails != null)
                {
                    //int imageCounter = GetMaxImageCounter(movieName);
                    int imageCounter = new BlobStorageService().GetImageFileCount(BlobStorageService.Blob_ImageContainer, movieName.Replace(" ", "-").ToLower() + "-poster-");

                    foreach (HtmlNode thumbnail in thumbnails)
                    {
                        if (thumbnail.Attributes["class"] != null && thumbnail.Attributes["class"].Value == "mb10")
                        {
                            string href = thumbnail.Attributes["src"].Value;
                            string newImageName = string.Empty;
                            string newPosterPath = GetNewImageName(movieName, GetFileExtension(href), imageCounter, false, ref newImageName);

                            posterPath.Add(newImageName);
                            DownloadImage(href, newPosterPath);
                            imageCounter++;
                        }
                    }
                }
            }

            return posterPath;
        }

        public string GetCleanRoleName(string castRole)
        {
            string roleName = castRole;
            castRole = castRole.ToLower();
            if (castRole.Contains("direct"))
            {
                roleName = "Director";
            }
            else if (castRole.Contains("cast"))
            {
                roleName = "Cast";
            }
            else if (castRole.Contains("writ"))
            {
                roleName = "Writer";
            }
            else if (castRole.Contains("produce"))
            {
                roleName = "Producer";
            }
            else if (castRole.Contains("music"))
            {
                roleName = "Music";
            }
            else if (castRole.Contains("cinemato"))
            {
                roleName = "Cinematography";
            }
            else if (castRole.Contains("edit"))
            {
                roleName = "Editor";
            }

            return roleName;
        }

        public void PopulateTeam(HtmlNode table, ref List<Cast> castList, string roleName)
        {
            var tbody = table.Element("tbody");

            if (tbody != null)
            {
                var tr = tbody.Elements("tr");
                if (tr != null)
                {
                    foreach (HtmlNode row in tr)
                    {
                        var nameNodes = row.Elements("td");
                        if (nameNodes != null)
                        {
                            Cast cast = new Cast();
                            cast.role = roleName.Replace("&nbsp;", " ");

                            foreach (HtmlNode node in nameNodes)
                            {
                                if (node.Attributes["class"] != null && node.Attributes["class"].Value == "name")
                                {
                                    var link = node.Element("a");
                                    cast.name = link.InnerText.Replace("'", string.Empty).Replace("&", string.Empty).Trim();

                                    if (link.Attributes["href"] != null)
                                    {
                                        cast.link = link.Attributes["href"].Value;
                                    }
                                }
                                else if (node.Attributes["class"] != null && node.Attributes["class"].Value == "credit")
                                {
                                    cast.charactername = node.InnerText.Replace("'", string.Empty).Replace("&", string.Empty).Trim();
                                }
                            }

                            castList.Add(cast);
                        }
                    }
                }
            }
        }

        public bool DownloadImage(string url, string filePath)
        {
            try
            {
                string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent: Other"); 
                    byte[] data = client.DownloadData(url);

                    using (Stream stream = new MemoryStream(data))
                    {
                        new BlobStorageService().UploadImageFileOnBlob(BlobStorageService.Blob_ImageContainer, fileName, stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }

            return true;
        }

        public string GetNewImageName(string movieName, string extension, int counter, bool isThumbnail, ref string newImageName)
        {
            if (string.IsNullOrEmpty(movieName) || string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            string tempImageName = string.Empty;
            try
            {
                tempImageName = movieName.Replace(" ", "-").ToLower();
                tempImageName += "-poster-";

                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                tempImageName = r.Replace(tempImageName, string.Empty);


                if (isThumbnail)
                {
                    tempImageName += "thumb-";
                    tempImageName += counter + extension;
                    newImageName = tempImageName;
                    //tempImageName = Path.Combine(PosterThumbnailPath, tempImageName);
                }
                else
                {
                    tempImageName += counter + extension;
                    newImageName = tempImageName;
                    //tempImageName = Path.Combine(PosterImagePath, tempImageName);
                }
            }
            catch (Exception)
            {
            }

            return tempImageName;
        }

        public void PopulateCast(HtmlNode table, ref List<Cast> castList, string roleName)
        {
            var tbody = table.Element("tbody");
            tbody = tbody ?? table;

            if (tbody != null)
            {
                var tr = tbody.Elements("tr");
                if (tr != null)
                {
                    foreach (HtmlNode row in tr)
                    {
                        var nameNodes = row.Elements("td");
                        if (nameNodes != null)
                        {
                            Cast cast = new Cast();
                            cast.role = roleName.Replace("&nbsp;", " ");

                            foreach (HtmlNode node in nameNodes)
                            {
                                if (node.Attributes["itemprop"] != null && node.Attributes["itemprop"].Value == "actor")
                                {
                                    var link = node.Element("a");
                                    cast.name = link.InnerText.Replace("'", string.Empty).Replace("&", string.Empty).Trim();

                                    if (link.Attributes["href"] != null)
                                        cast.link = link.Attributes["href"].Value;
                                }
                                else if (node.Attributes["class"] != null && node.Attributes["class"].Value == "character")
                                {
                                    cast.charactername = node.InnerText.Replace("'", string.Empty).Replace("&", string.Empty).Trim();
                                }
                            }

                            if (!string.IsNullOrEmpty(cast.charactername) || !string.IsNullOrEmpty(cast.name))
                            {
                                castList.Add(cast);
                            }
                        }
                    }
                }
            }
        }

        public string GetFileExtension(string url)
        {
            //int lastIndex = url.
            // TODO: need to parse the url and get the extension from the url
            return ".jpg";
        }

        /*public int GetMaxImageCounter(string movieName)
        {
            string tempImageName = movieName.Replace(" ", "-").ToLower();
            tempImageName += "-poster-*";
            int maxId = 0;

            try
            {
                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex reg = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                //tempImageName = reg.Replace(tempImageName, string.Empty);

                DirectoryInfo dir = new DirectoryInfo(PosterImagePath);
                FileInfo[] files = dir.GetFiles(tempImageName);
                foreach (FileInfo file in files)
                {
                    int id = 0;
                    string fileName = file.Name.Substring(file.Name.LastIndexOf("-") + 1);
                    fileName = fileName.Substring(0, fileName.IndexOf("."));
                    int.TryParse(fileName, out id);
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("An error occurred while getting the max poster count for movie named - " + movieName);
            }

            return maxId + 1;
        }*/
    }
}
