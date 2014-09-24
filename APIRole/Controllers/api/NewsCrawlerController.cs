

namespace CloudMovie.APIRole.Controllers.api
{
    using CloudMovie.APIRole.API;
    using Crawler;
    using DataStoreLib.BlobStorage;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;

    public class NewsCrawlerController : BaseController
    {
        protected override string ProcessRequest()
        {
            try
            {
                BlobStorageService _blobStorageService = new BlobStorageService();

                string newsXmlFileBlobPath = _blobStorageService.GetSinglFile(BlobStorageService.Blob_XMLFileContainer, "News.xml");

                GetNews(newsXmlFileBlobPath);

                return "{ \"Status\":\"Ok\",\"Message\" : \"successfully crawl news.\" }";
            }
            catch (Exception ex)
            {
                return "{ \"Status\":\"Error\", \"Message\" : \"Error occured.\", \"ActualMessage\" : \"" + ex.Message + "\"}";
            }
        }

        public void GetNews(string blobXmlFilePath = "")
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                string newsXml = string.Empty;

                if (blobXmlFilePath == "")
                {
                    string basePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MovieList"]);
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
    }
}
