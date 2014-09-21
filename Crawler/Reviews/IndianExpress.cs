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
    public class IndianExpress
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
                Debug.WriteLine(string.Format("Exception occored while getting reviews (Indian Express), message= {0}", ex.Message));
            }

            return null;
        }

        public ReviewEntity PopulateReviewDetail(string html, string affiliation)
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
                    Console.WriteLine("Body is empty");
                }
                else
                {
                    try
                    {
                        // Rating
                        var ratingNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "story-rating");
                        var imageContainer = ratingNode.Element("span");
                        var ratingImages = imageContainer.Elements("img");
                        int rate = 0;
                        foreach (var rateImage in ratingImages)
                        {
                            HtmlAttribute src = rateImage.Attributes["src"];

                            if (src != null)
                            {
                                if (src.Value.Contains("star-one-1"))
                                {
                                    rate += 1;
                                }

                                #region Commented Code
                                /*else if (src.Value.Contains("star-no-1"))
                            {
                            // We don't need to add any rating because its 0
                            }
                            else
                            {
                                // This case could be 0.5 but not sure how it appears on review page. 
                                // Need to wait for same for other movie reviews
                            }*/
                                #endregion
                            }
                        }

                        rate = rate * 2;

                        // Reviewer
                        var reviewerNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "editor");
                        var reviewerName = reviewerNode.Element("a");//, "class", "fn");
                        var reviewName = reviewerName == null ? string.Empty : reviewerName.InnerText;

                        // Review Text
                        var reviewBody = helper.GetElementWithAttribute(bodyNode, "div", "class", "main-body-content");
                        var reviewText = reviewBody == null ? string.Empty : reviewBody.InnerText;

                        re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                        re.Affiliation = affiliation.Trim();
                        re.Review = reviewText.Replace("&#39;", "'").Trim();
                        re.ReviewerName = reviewName.Trim();
                        re.ReviewerRating = rate.ToString();
                        re.MyScore = string.Empty;
                        re.JsonString = string.Empty;
                        return re;
                    }
                    catch (Exception)
                    {
                        // Log an exception
                    }
                }
            }

            return null;
        }
    }
}
