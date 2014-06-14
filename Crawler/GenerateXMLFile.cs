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
        public string CreatingFile(string filePath, XMLMovieProperties objMovie)
        {
            try
            {
                if (objMovie == null) return null;

                XmlDocument documnet = new XmlDocument();

                string fileName = "MovieList-" + objMovie.Month.Substring(0, 3) + "-" + objMovie.Year.ToString() + ".xml";

                filePath = Path.Combine(filePath, fileName);

                if (File.Exists(filePath))
                {
                    documnet.Load(filePath);

                    var oldMonth = documnet.SelectSingleNode("Movies/Month[@name='" + objMovie.Month + "']");

                    var oldMovie = oldMonth.SelectSingleNode("Movie[@name='" + objMovie.MovieName + "']");

                    if (oldMovie == null)
                        oldMonth.AppendChild(AddMovieNode(documnet, objMovie));
                    else
                    {
                        //UpdateMovieNode(documnet, oldMovie, objMovie);
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

                documnet.Save(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
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
                
            }

            return null;
        }
    }
}
