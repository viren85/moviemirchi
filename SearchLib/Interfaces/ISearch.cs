using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Interfaces
{
    public interface ISearch
    {
        void GetAllMoviesWith(string textSearch, int maxCount, out List<string> movies, out List<string> reviews, IList<string> filters = null);
    }
}
