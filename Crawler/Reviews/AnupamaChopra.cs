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
    public class AnupamaChopra
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
                Debug.WriteLine(string.Format("Exception occored while getting reviews (Anupama Chopra), message= {0}", ex.Message));
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
                    var headerNode = helper.GetElementWithAttribute(bodyNode, "h4", "class", "book_page_title");
                    //HtmlNode head = headerNode.SelectSingleNode("h1");
                    var header = headerNode == null ? string.Empty : headerNode.InnerText;

                    var reviewContentNode = helper.GetElementWithAttribute(bodyNode, "div", "class", "book_para");
                    //HtmlNodeCollection nodes = reviewContentNode.SelectNodes("p");
                    HtmlNodeCollection nodes = reviewContentNode.SelectNodes("div");
                    var review = string.Empty;
                    foreach (var ratingNode in nodes)
                    {
                        review += ratingNode.InnerText;
                    }

                    var reviewerRating = helper.GetElementWithAttribute(bodyNode, "article", "class", "floatl w591px");
                    //HtmlNodeCollection rateImg = reviewerRating.SelectNodes("figure");
                    if (reviewerRating != null)
                    {
                        try
                        {
                            rating = PrepareRatingValue(reviewerRating);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    re.RowKey = re.ReviewId = Guid.NewGuid().ToString();
                    re.Affiliation = affiliation.Trim();
                    re.Review = review.Trim();
                    re.ReviewerName = "Anupama Chopra";
                    re.ReviewerRating = rating;
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
            //var reviewContentNode = helper.GetElementWithAttribute(ratingNode, "img", "class", "imgwidth");
            HtmlNodeCollection figures = ratingNode.SelectNodes("figure");
            foreach (var fig in figures)
            {
                HtmlNodeCollection ratingContentNodes = fig.SelectNodes("img");
                if (ratingContentNodes != null) {
                foreach (var ratingContentNode in ratingContentNodes)
                {
                    HtmlAttribute src = ratingContentNode.Attributes["src"];
                    imageSrc = src.Value;
                    if (imageSrc != null)
                    {
                        try
                        {

                            bool fullPoint = Regex.IsMatch(imageSrc, "or");
                            bool halfPoint = Regex.IsMatch(imageSrc, "_gr_or");
                            if (fullPoint)
                            {
                                rate += 1;
                              
                            }
                            else if(halfPoint)
                            {
                                rate += 0.5;
                                
                            }

                            //rate = rate;
                        }
                        catch
                        {
                        }
                    }
                }
                }
            }
            rate = rate * 2;
            return rate.ToString();
        }
    }
}
