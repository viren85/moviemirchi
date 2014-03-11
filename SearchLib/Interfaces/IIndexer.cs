using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Interfaces
{
    public interface IIndexer
    {
        void IndexSelectedMovies(ISet<string> movieIds);
        void IndexSelectedReviews(ISet<string> reviewIds);
    }
}
