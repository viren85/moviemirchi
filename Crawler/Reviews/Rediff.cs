using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Crawler.Reviews
{
    public class Rediff
    {
        private CrawlerHelper helper = new CrawlerHelper();
        string reviewPageContent = string.Empty;
        public ReviewEntity Crawl(string url, string affiliation)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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

                    reviewPageContent = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();

                    return PopulateReviewDetail(reviewPageContent, affiliation);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Exception occored while getting reviews (Rediff), message= {0}", ex.Message));
            }

            return null;
        }

        public ReviewEntity PopulateReviewDetail(string html, string affiliation)
        {
            ReviewEntity re = new ReviewEntity();
            string rating = string.Empty;
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("Body is empty");
                }
                else
                {
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "h1", "class", "arti_heading");
                    var header = headerNode == null ? headerNode.InnerHtml : headerNode.InnerText;
                    var reviewerName = helper.GetElementWithAttribute(bodyNode, "span", "class", "grey1");
                    var reviewName = reviewerName == null ? string.Empty : reviewerName.InnerText;
                    var reviewContentNode = helper.GetElementWithAttribute(bodyNode, "div", "itemprop", "articleBody");
                    HtmlNodeCollection nodes = reviewContentNode.SelectNodes("p");
                    var review = string.Empty;
                    var reviewerRating = string.Empty;
                    foreach (var ratingNode in nodes)
                    {
                        review += ratingNode.InnerText;

                        if (ratingNode.InnerText.ToLower().Contains("rating"))
                        {
                            reviewerRating = PrepareRatingValue(ratingNode);
                        }
                    }

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Replace("&#39;", "'").Trim();
                    re.ReviewerName = reviewName.Trim();
                    re.ReviewerRating = reviewerRating;
                    re.MyScore = string.Empty;
                    re.JsonString = string.Empty;
                    return re;
                }
            }

            return null;
        }

        // added for getting the value from the star image gif file
        public string PrepareRatingValue(HtmlNode ratingNode)
        {
            double rate = 0;
            string imageSrc = string.Empty;
            var reviewContentNode = helper.GetElementWithAttribute(ratingNode, "img", "class", "imgwidth");
            HtmlAttribute src = reviewContentNode.Attributes["src"];
            imageSrc = src.Value;
            if (imageSrc != null)
            {
                try
                {
                    string[] numbers = Regex.Split(imageSrc, @"\D+");

                    foreach (string value in numbers)
                    {

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (rate ==  0)
                            {
                                if (double.Parse(value) > 0 && double.Parse(value) < 9)
                                {
                                    rate = double.Parse(value);
                                }
                            }
                        }
                    }

                    if (imageSrc.ToLower().Contains("half"))
                    {
                        rate += 0.5;
                        rate = rate * 2;
                    }
                }
                catch (Exception)
                {
                }
            }
            return rate.ToString();
        }
    }
}
