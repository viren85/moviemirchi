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
    public class Ndtv
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
                Debug.WriteLine(string.Format("Exception occored while getting reviews (NDTV), message= {0}", ex.Message));
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
                    // Reviewer Name
                    // Reviewer Rating
                    // Review Text

                    var headerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "ndmv-celeb-detail-bread ndmv-review-breadcrumb");
                    HtmlNode node = headerNode.SelectSingleNode("div");
                    var header = node == null ? string.Empty : node.InnerText;

                    var reviewerName = helper.GetElementWithAttribute(node, "a", "class", "fn");
                    var reviewName = reviewerName == null ? string.Empty : reviewerName.InnerText;

                    var reviewContentNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "row ndmv-celeb-detail-info ndmv-review-detail");
                    var contentNode = helper.GetElementWithAttribute(reviewContentNode, "div", "class", "col-md-16");

                    var textNode = contentNode.Elements("p");

                    var reviews = contentNode == null ? string.Empty : textNode.FirstOrDefault().InnerText.Replace("SPOILERS ALERT", string.Empty);

                    var reviewerRating = string.Empty;
                    var reviewRating = helper.GetElementWithAttribute(bodyNode, "div", "class", "ndmv-movie-rating");
                    if (reviewRating != null)
                    {
                        reviewerRating = PrepareRatingValue(reviewRating);
                    }
                    else
                    {
                        reviewerRating = string.Empty;
                    }

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = reviews.Trim();
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

            try
            {
                HtmlNodeCollection ratingContentNodes = ratingNode.SelectNodes("span");
                if (ratingContentNodes != null)
                {
                    foreach (var ratingContentNode in ratingContentNodes)
                    {
                        HtmlAttribute src = ratingContentNode.Attributes["class"];
                        switch (src.Value)
                        {
                            case "rating-full":
                                rate += 1;
                                break;
                            case "rating-half":
                                rate += 0.5;
                                break;
                            case "rating-null":
                                rate += 0;
                                break;

                        }
                    }
                }

                rate = rate * 2;
            }
            catch (Exception ex)
            {
                // Log an exception
            }

            return rate.ToString();
        }
    }
}
