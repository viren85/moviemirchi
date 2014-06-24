namespace Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System.Net;
    using System.IO;
    using HtmlAgilityPack;
    using Newtonsoft.Json;

    public class ArtistCrawler
    {
        private CrawlerHelper helper = new CrawlerHelper();

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
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cast.link);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        #region Get Artist Page Content
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                        artistPageContent = readStream.ReadToEnd();

                        response.Close();
                        readStream.Close();
                        #endregion
                    }

                    ArtistEntity artist = PopulateArtistsDetails(artistPageContent, cast.link);
                    aeList.Add(artist);
                }

                tblMgr.UpdateArtistItemById(aeList);
            }
            catch (Exception ex)
            {

            }
        }

        public ArtistEntity PopulateArtistsDetails(string html, string url)
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
            return JsonConvert.SerializeObject(posters); ;
        }
    }
}
