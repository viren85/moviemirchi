using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Reviews
{
    public class FilmfareReviews
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

                    PopulateReviewDetails(reviewPageContent, affiliation);

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
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "pageContent");

                    #region Get Reviewer
                    var reviewerNameContainer = helper.GetElementWithAttribute(headerNode, "span", "class", "written");
                    var reviewerList = reviewerNameContainer.Elements("a");
                    string reviewerName = string.Empty;
                    
                    if (reviewerList != null)
                    {
                        try
                        {
                            reviewerName = reviewerList.FirstOrDefault().InnerText;
                        }
                        catch (Exception ex) { }
                    }
                    #endregion

                    #region Get Rating
                    var ratingNode = helper.GetElementWithAttribute(bodyNode, "span", "id", "rate_val_change");
                    var rating = ratingNode.Attributes["class"] != null ? ratingNode.Attributes["class"].Value : string.Empty;
                    rating = rating.Replace("rate", "");
                    #endregion

                    #region Get Review Content
                    var reviewContent = helper.GetElementWithAttribute(bodyNode, "div", "class", "upperBlk");
                    var review = reviewContent.InnerText;
                    #endregion

                    re.Affiliation = affiliation;
                    re.Review = review;
                    re.ReviewerName = reviewerName;
                    re.ReviewerRating = rating.ToString();

                    return re;
                }
            }

            return null;
        }
    }
}
