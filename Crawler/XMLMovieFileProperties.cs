using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class XMLMovieProperties
    {
        public string MovieName { get; set; }
        public string MovieLink { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public List<XMLReivewProperties> Reviews { get; set; }
    }

    public class XMLReivewProperties
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
