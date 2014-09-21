namespace Crawler
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using HtmlAgilityPack;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class ArtistCrawler
    {
        private CrawlerHelper helper = new CrawlerHelper();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public void CrawlArtists(List<Cast> castItems)
        {
            if (castItems == null)
            {
                return;
            }

            try
            {
                string artistPageContent = string.Empty;

                TableManager tblMgr = new TableManager();
                List<ArtistEntity> aeList = new List<ArtistEntity>();

                foreach (Cast cast in castItems)
                {
                    if (string.IsNullOrEmpty(cast.link)) continue;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cast.link);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            #region Get Artist Page Content
                            using (Stream receiveStream = response.GetResponseStream())
                            {
                                using (StreamReader readStream =
                                    response.CharacterSet == null ?
                                        new StreamReader(receiveStream)
                                        : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet)))
                                {
                                    artistPageContent = readStream.ReadToEnd();
                                }
                            }
                            #endregion
                        }
                    }

                    ArtistEntity artist = PopulateArtistsDetails(artistPageContent, cast.link);
                    aeList.Add(artist);
                }

                //tblMgr.UpdateArtistItemById(aeList);

                foreach (ArtistEntity obj in aeList)
                {
                    tblMgr.UpdateArtistById(obj);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public ArtistEntity PopulateArtistsDetails(string html, string url)
        {
            try
            {
                ArtistEntity artist = new ArtistEntity();
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
                        artist.RowKey = artist.ArtistId = Guid.NewGuid().ToString();
                        artist.ArtistName = GetArtistName(bodyNode);
                        artist.UniqueName = artist.ArtistName.Replace(" ", "-");
                        artist.Bio = GetArtistBio(bodyNode);
                        artist.Born = GetArtistBirthDetails(bodyNode);
                        artist.MovieList = GetMovieList(bodyNode);
                        artist.Posters = GetArtistPosters(url + "mediaindex", artist.UniqueName, bodyNode);
                        artist.Popularity = Util.DEFAULT_POPULARITY;
                        artist.MyScore = Util.DEFAULT_SCORE;
                        artist.JsonString = string.Empty;
                    }
                }

                return artist;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }
        }

        private string GetArtistName(HtmlNode body)
        {
            var headerNode = helper.GetElementWithAttribute(body, "h1", "class", "header");
            var movieName = helper.GetElementWithAttribute(headerNode, "span", "class", "itemprop");
            return (movieName == null) ? string.Empty : movieName.InnerText;
        }

        private string GetArtistBio(HtmlNode body)
        {
            var bioNode = helper.GetElementWithAttribute(body, "div", "id", "name-bio-text");
            var artistBio = helper.GetElementWithAttribute(bioNode, "div", "itemprop", "description");
            return (artistBio == null) ? string.Empty : artistBio.InnerText;
        }

        private string GetArtistBirthDetails(HtmlNode body)
        {
            var birthNode = helper.GetElementWithAttribute(body, "div", "id", "name-born-info");
            return (birthNode == null) ? string.Empty : birthNode.InnerText;
        }

        private string GetMovieList(HtmlNode body)
        {
            return string.Empty;
        }

        private string GetArtistPosters(string url, string artistName, HtmlNode body)
        {
            string thumbnailPath = string.Empty;
            MovieCrawler movieCrawler = new MovieCrawler();
            List<string> posters = movieCrawler.CrawlPosters(url, artistName, ref thumbnailPath);
            return JsonConvert.SerializeObject(posters);
        }
    }
}
