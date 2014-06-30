using DataStoreLib.BlobStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Crawler
{
    public class GenerateXMLFile
    {
        public string CreatingFile(XMLMovieProperties objMovie)
        {
            try
            {
                if (objMovie == null) return null;

                BlobStorageService _blobStorageService = new BlobStorageService();
                XmlDocument documnet = new XmlDocument();

                string fileName = "MovieList-" + objMovie.Month.Substring(0, 3) + "-" + objMovie.Year.ToString() + ".xml";
                string existFileContent = _blobStorageService.GetUploadeXMLFileContent(BlobStorageService.Blob_XMLFileContainer, fileName);

                if (!string.IsNullOrEmpty(existFileContent))
                {
                    documnet.LoadXml(existFileContent);

                    var oldMonth = documnet.SelectSingleNode("Movies/Month[@name='" + objMovie.Month + "']");

                    var oldMovie = oldMonth.SelectSingleNode("Movie[@name='" + objMovie.MovieName + "']");

                    if (oldMovie == null)
                        oldMonth.AppendChild(AddMovieNode(documnet, objMovie));
                    else
                    {
                        oldMonth.RemoveChild(oldMovie);
                        oldMonth.AppendChild(AddMovieNode(documnet, objMovie));
                    }
                }
                else
                {
                    XmlNode root = documnet.CreateNode(XmlNodeType.Element, "Movies", "");

                    XmlAttribute movieYear = documnet.CreateAttribute("year");
                    movieYear.Value = objMovie.Year.ToString();
                    root.Attributes.Append(movieYear);

                    XmlNode month = documnet.CreateNode(XmlNodeType.Element, "Month", "");

                    XmlAttribute monthName = documnet.CreateAttribute("name");
                    monthName.Value = objMovie.Month.ToString();
                    month.Attributes.Append(monthName);

                    month.AppendChild(AddMovieNode(documnet, objMovie));
                    root.AppendChild(month);
                    documnet.AppendChild(root);
                }

                _blobStorageService.UploadXMLFileOnBlob(BlobStorageService.Blob_XMLFileContainer, fileName, documnet.OuterXml);

                return documnet.OuterXml;
                //return fileName;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }

        private XmlNode AddMovieNode(XmlDocument documnet, XMLMovieProperties objMovie)
        {
            XmlNode movie = documnet.CreateNode(XmlNodeType.Element, "Movie", "");

            XmlAttribute movieName = documnet.CreateAttribute("name");
            movieName.Value = objMovie.MovieName.ToString();
            movie.Attributes.Append(movieName);

            XmlAttribute movieLink = documnet.CreateAttribute("link");
            movieLink.Value = objMovie.MovieLink.ToString();
            movie.Attributes.Append(movieLink);

            foreach (XMLReivewProperties xmlReviews in objMovie.Reviews)
            {
                XmlNode review = documnet.CreateNode(XmlNodeType.Element, "Review", "");

                XmlAttribute reviewName = documnet.CreateAttribute("name");
                reviewName.Value = xmlReviews.Name.ToString();
                review.Attributes.Append(reviewName);

                XmlAttribute reviewLink = documnet.CreateAttribute("link");
                reviewLink.Value = xmlReviews.Link.ToString();
                review.Attributes.Append(reviewLink);

                movie.AppendChild(review);
            }

            return movie;
        }

        private XmlNode UpdateMovieNode(XmlDocument document, XmlNode movieNode, XMLMovieProperties objMovie)
        {
            try
            {
                movieNode.Attributes["name"].Value = objMovie.MovieName;
                movieNode.Attributes["link"].Value = objMovie.MovieLink;

                movieNode.RemoveAll();

                foreach (XMLReivewProperties xmlReviews in objMovie.Reviews)
                {
                    XmlNode review = document.CreateNode(XmlNodeType.Element, "Review", "");

                    XmlAttribute reviewName = document.CreateAttribute("name");
                    review.Value = xmlReviews.Name.ToString();
                    review.Attributes.Append(reviewName);

                    XmlAttribute reviewLink = document.CreateAttribute("link");
                    reviewLink.Value = xmlReviews.Link.ToString();
                    review.Attributes.Append(reviewLink);

                    movieNode.AppendChild(review);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return null;
        }

        public List<XMLMovieProperties> GetMoviesFromXml(string[] files)
        {
            try
            {
                BlobStorageService _blobStorageService = new BlobStorageService();

                List<XMLMovieProperties> movieList = new List<XMLMovieProperties>();

                foreach (string file in files)
                {
                    // get xml file data from blob
                    string xmlData = _blobStorageService.GetUploadeXMLFileContent(BlobStorageService.Blob_XMLFileContainer, file);

                    if (string.IsNullOrEmpty(xmlData)) continue;

                    XmlDocument documnet = new XmlDocument();

                    try
                    {
                        documnet.LoadXml(xmlData);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    var root = documnet.SelectSingleNode("Movies");
                    var monthNode = root.SelectSingleNode("Month");
                    var movieNodes = monthNode.SelectNodes("Movie");

                    foreach (XmlNode movieNode in movieNodes)
                    {
                        XMLMovieProperties singleMovie = new XMLMovieProperties();
                        singleMovie.MovieId = Guid.NewGuid().ToString();
                        singleMovie.Month = monthNode.Attributes["name"].Value;
                        singleMovie.Year = Convert.ToInt32(root.Attributes["year"].Value);

                        singleMovie.MovieName = movieNode.Attributes["name"].Value;
                        singleMovie.MovieLink = movieNode.Attributes["link"].Value;

                        var reviewNodes = movieNode.SelectNodes("Review");

                        List<XMLReivewProperties> reviewList = new List<XMLReivewProperties>();

                        foreach (XmlNode reviewNode in reviewNodes)
                        {
                            XMLReivewProperties review = new XMLReivewProperties();

                            review.Name = reviewNode.Attributes["name"].Value;
                            review.Link = reviewNode.Attributes["link"].Value;

                            reviewList.Add(review);
                        }

                        singleMovie.Reviews = reviewList;

                        movieList.Add(singleMovie);
                    }
                }

                return movieList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
