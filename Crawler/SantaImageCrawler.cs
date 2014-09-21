
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

    public class SantaImageCrawler
    {
        private static readonly string PosterPath = Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "Posters");
        private static readonly string PosterImagePath = Path.Combine(PosterPath, "Images");
        private static readonly string PosterThumbnailPath = Path.Combine(PosterPath, "Thumbnails");

        private CrawlerHelper helper = new CrawlerHelper();

        // 1. Crawl main page to get the link of pictures - movies
        // 2. Download each image & save it. Rename the downloaded images
        // 3. 

        // This method crawls the SantaBanta Movie Page. It expects the base URL of sanatbanta movie page
        public List<string> GetMoviePosterUrls(string movieBaseUrl)
        {
            try
            {
                string body = GetPageBody(movieBaseUrl);

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(body);
                if (htmlDoc.DocumentNode != null)
                {
                    return GetMoviePoster(htmlDoc.DocumentNode);
                }
            }
            catch (Exception)
            {
                // TODO - Log an exception
            }

            return null;
        }

        private string GetPageBody(string url)
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

                        return readStream.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                // TODO - Log an exception message here
            }

            return string.Empty;
        }

        // Posters
        //get the poster link
        public List<string> GetMoviePoster(HtmlNode body)
        {
            List<string> posters = new List<string>();
            try
            {
                var container = helper.GetElementWithAttribute(body, "div", "class", "content-div-new");
                var elements = helper.GetElementWithAttribute(container, "div", "class", "home-cinema-box-new");
                var posterContainer = helper.GetElementWithAttribute(elements, "div", "class", "wallpaper-big-1");
                var imageContainers = posterContainer.Elements("div");

                foreach (var imageContainer in imageContainers)
                {
                    if (imageContainer.Attributes["class"] != null && imageContainer.Attributes["class"].Value.Contains("wallpapers-box-300x180-2"))
                    {
                        var cont = helper.GetElementWithAttribute(imageContainer, "div", "class", "wallpapers-box-300x180-2-img");
                        var a = cont.Element("a");
                        var img = a.Element("img");

                        if (img.Attributes["src"] != null && !string.IsNullOrEmpty(img.Attributes["src"].Value))
                            posters.Add(img.Attributes["src"].Value);

                    }

                }

                return posters;
            }
            catch (Exception)
            {
                // TODO - Log error message
            }

            return null;
        }

        /*public string GetMoviePictures(HtmlNode body)
        {
            return string.Empty;
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

                        response.Close();
                        readStream.Close();
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
        }*/
    }
}
