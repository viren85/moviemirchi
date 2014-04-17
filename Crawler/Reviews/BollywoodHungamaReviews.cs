using DataStoreLib.Models;
using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Reviews
{
    public class BollywoodHungamaReviews
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
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "id", "celeb_article_postview_tab");
                    var reviewrName = helper.GetElementWithAttribute(headerNode, "div", "class", "m9090");
                    var reviewName = reviewrName.InnerText;

                    // Clean up the review name - It has lots of scrape values along with name
                    if (!string.IsNullOrEmpty(reviewName))
                    {
                        
                        reviewName = reviewName.Replace("&nbsp;", " ").Replace("By", "").Trim();
                        int nameLength = reviewName.IndexOf(",");

                        if (nameLength > 1)
                        {
                            reviewName = reviewName.Substring(0, nameLength);
                        }
                    }

                    var ratingNode = helper.GetElementWithAttribute(reviewrName, "img", "width", "93");
                    var rating = ratingNode.Attributes["title"] != null ? ratingNode.Attributes["title"].Value : string.Empty;

                    float multipliedRating = 0;

                    float.TryParse(rating, out multipliedRating);

                    if (multipliedRating > 0)
                    {
                        // All other rating are based out of 10 where as Filmfare is out of 5.
                        rating = (multipliedRating * 2).ToString();
                    }

                    var reviewContent = helper.GetElementWithAttribute(headerNode, "div", "class", " mfl mmb31 mfnt12 minline malignjus mmr18");
                    var review = reviewContent.InnerText;

                    re.Affiliation = affiliation;
                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Review = review;
                    re.ReviewerName = reviewName;
                    re.ReviewerRating = rating.ToString();
                    return re;
                }
            }

            return null;
        }
    }
}
