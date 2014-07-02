using DataStoreLib.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Reviews
{
    public class MumbaiMirror
    {
        private CrawlerHelper helper = new CrawlerHelper();
        string reviewPageContent = string.Empty;

        /// <summary>
        /// Entry point for the crawler. Pass the URL from source file (XML)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ReviewEntity Crawl(string url, string affiliation)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

                    reviewPageContent = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                    #endregion

                    return PopulateReviewDetails(reviewPageContent, affiliation);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Exception occored while getting reviews (Mumbai Mirror), message= {0}", ex.Message));
            }

            return null;
        }

        private ReviewEntity PopulateReviewDetails(string html, string affiliation)
        {
            ReviewEntity re = new ReviewEntity();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode == null)
                {
                    Console.WriteLine("body node is null");
                }
                else
                {
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "Normal");
                    var reviewerNode = helper.GetElementWithAttribute(headerNode, "span", "id", "advenueINTEXT");
                    string reviewerName = string.Empty;

                    if (reviewerNode != null)
                    {
                        reviewerName = reviewerNode.InnerText;
                    }

                    // Clean up the review name - It has lots of scrape values along with name
                    if (!string.IsNullOrEmpty(reviewerName))
                    {
                        reviewerName = reviewerName.Replace("&nbsp;", " ").Replace("By", "").Trim();
                    }

                    HtmlNodeCollection nodes = headerNode.SelectNodes("strong");

                    var rating = string.Empty;

                    foreach (var node in nodes)
                    {
                        if (node.InnerText.ToLower().Trim().Contains("rating:"))
                        {
                            rating = node.InnerText.Replace(" ", "").Replace("Rating:", "").Length.ToString();
                        }

                        if (string.IsNullOrEmpty(reviewerName) && node.InnerText.ToLower().Trim().Contains("by:"))
                        {
                            reviewerName = rating = node.InnerText.Replace(" ", "").Replace("By:", "");
                        }
                        else if (string.IsNullOrEmpty(reviewerName))
                        {
                            reviewerName = "mumbaimirror";
                        }
                    }

                    float multipliedRating = 0; 

                    float.TryParse(rating, out multipliedRating);

                    if (multipliedRating > 0)
                    {
                        // All other rating are based out of 10 where as Filmfare is out of 5.
                        rating = (multipliedRating * 2).ToString();
                    }

                    var review = string.Empty;

                    if (!string.IsNullOrEmpty(rating))
                        review = headerNode.InnerText.Substring(headerNode.InnerText.LastIndexOf("Rating:") + rating.Length + 1);
                    else if (!string.IsNullOrEmpty(reviewerName) && reviewerName != "mumbaimirror")
                        review = headerNode.InnerText.Substring(headerNode.InnerText.LastIndexOf(reviewerName) + rating.Length + 1);
                    else
                        review = headerNode.InnerText;

                    re.Affiliation = affiliation;
                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Review = review.Trim();
                    re.ReviewerName = reviewerName.Trim();
                    re.ReviewerRating = rating.ToString();
                    re.MyScore = string.Empty;
                    re.JsonString = string.Empty;

                    return re;
                }
            }

            return null;
        }
    }
}
