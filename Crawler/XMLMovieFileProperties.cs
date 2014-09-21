using DataStoreLib.BlobStorage;
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

        public string SantaPosterLink { get; set; }
        public string SaavnSongLink { get; set; }

        public int Year { get; set; }
        public string Month { get; set; }
        public List<XMLReivewProperties> Reviews { get; set; }

        public List<XMLMovieProperties> GetMovieListFromXMLFiles(bool isFromLatest)
        {
            try
            {
                GenerateXMLFile objGenerateXml = new GenerateXMLFile();

                if (isFromLatest)
                {
                    string fileName = "MovieList-" + DateTime.Now.ToString("MMM-yyyy") + ".xml";

                    return objGenerateXml.GetMoviesFromXml(new string[] { fileName });
                }
                else
                {
                    string[] files = new BlobStorageService().GetUploadedFileFromBlob("crawlfiles").ToArray();
                    
                    return objGenerateXml.GetMoviesFromXml(files);
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }

    public class XMLReivewProperties
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
