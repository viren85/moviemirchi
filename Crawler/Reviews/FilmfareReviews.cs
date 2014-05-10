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
                        catch (Exception)
                        {
                        }
                    }
                    #endregion

                    #region Get Rating
                    var ratingNode = helper.GetElementWithAttribute(bodyNode, "span", "id", "rate_val_change");
                    var rating = ratingNode.Attributes["class"] != null ? ratingNode.Attributes["class"].Value : string.Empty;
                    rating = rating.Replace("rate", "");
                    float multipliedRating = 0;

                    float.TryParse(rating, out multipliedRating);

                    if (multipliedRating > 0)
                    {
                        // All other rating are based out of 10 where as Filmfare is out of 5.
                        rating = (multipliedRating * 2).ToString();
                    }

                    #endregion

                    #region Get Review Content
                    var reviewContent = helper.GetElementWithAttribute(bodyNode, "div", "class", "upperBlk");
                    var reviews = reviewContent.Element("figure");
                    string review = string.Empty;

                    if (reviews != null)
                    {
                        var reviewElements = reviews.Elements("p");

                        foreach (var r in reviewElements)
                        {
                            if (!string.IsNullOrEmpty(r.InnerText) && r.InnerText.Length > 300)
                            {
                                review = r.InnerText;
                                break;
                            }
                        }
                    }
                    #endregion

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Trim();
                    re.ReviewerName = reviewerName.Trim();
                    re.ReviewerRating = rating.ToString();
                    re.JsonString = string.Empty;
                    re.MyScore = string.Empty;
                    return re;
                }
            }

            return null;
        }
    }
}
