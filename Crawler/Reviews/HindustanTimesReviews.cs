using DataStoreLib.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Reviews
{
    public class HindustanTimesReviews
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
            catch (Exception)
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
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "story_wid");
                    var reviewerName = helper.GetElementWithAttribute(headerNode, "span", "class", "sty_agn");

                    HtmlNode node = reviewerName.Element("a");
                    var reviewName = node == null ? reviewerName.InnerHtml : node.InnerText;

                    var reviewContent = helper.GetElementWithAttribute(headerNode, "div", "class", "sty_txt");
                    var review = reviewContent.InnerText;

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Trim();
                    re.ReviewerName = reviewName.Trim();
                    re.ReviewerRating = string.Empty;

                    return re;
                }
            }

            return null;
        }
    }
}
