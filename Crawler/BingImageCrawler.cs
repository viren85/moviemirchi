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
   using System.Threading.Tasks;


   public class BingCrawler
    {
        private CrawlerHelper helper = new CrawlerHelper();

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

        // Get the poster link
        public List<string> GetMoviePoster(HtmlNode body)
        {
            List<string> posters = new List<string>();
            try
            {
                var container = helper.GetElementWithAttribute(body, "div", "class", "norr");
                var elements = helper.GetElementWithAttribute(container, "div", "class", "dg_b");
                var posterContainer = helper.GetElementWithAttribute(elements, "div", "class", "imgres");
                var imageContainers = posterContainer.Elements("div");

                foreach (var imageContainer in imageContainers)
                {
                    if (imageContainer.Attributes["class"] != null && imageContainer.Attributes["class"].Value.Contains("dg_u"))
                    {
                        var cont = helper.GetElementWithAttribute(imageContainer, "div", "class", "dg_u");
                        var a = imageContainer.Element("a");
                        var img = a.Element("img");

                        string url = a.Attributes["m"].Value;

                        url = WebUtility.HtmlDecode(url);

                        dynamic obj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(url, typeof(Object));


                        url = obj["imgurl"];
                            //System.Web.Script.Serialization.Javascriptserialization
                        if (url != null && !string.IsNullOrEmpty(url))
                            posters.Add(url);

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

    } 
} 
