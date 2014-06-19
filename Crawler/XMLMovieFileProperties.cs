using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class XMLMovieProperties
    {
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieLink { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public List<XMLReivewProperties> Reviews { get; set; }

        public List<XMLMovieProperties> GetMovieListFromXMLFiles(string filePath, bool isFromLatest)
        {
            try
            {
                GenerateXMLFile objGenerateXml = new GenerateXMLFile();

                if (isFromLatest)
                {
                    string fileName = "MovieList-" + DateTime.Now.ToString("MMM-yyyy") + ".xml";

                    filePath = Path.Combine(filePath, fileName);

                    if (File.Exists(filePath))
                    {
                        return objGenerateXml.GetMoviesFromXml(new string[] { filePath });
                    }
                }
                else
                {
                    string[] files = Directory.GetFiles(filePath, "MovieList-*");
                    return objGenerateXml.GetMoviesFromXml(files);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }

    public class XMLReivewProperties
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
