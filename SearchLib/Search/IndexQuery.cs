using System.Diagnostics;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SearchLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.QueryParsers;
using Version = Lucene.Net.Util.Version;

namespace SearchLib.Search
{
    public class IndexQuery : ISearch
    {
        protected string _location;
        protected IndexSearcher _searcher;
        protected readonly int MaxCount = 100;

        public static IndexQuery GetIndexReader(string location)
        {
            var indexer = new IndexQuery(location);
            indexer.LoadIndex();

            return indexer;
        }

        protected IndexQuery(string indexLocation)
        {
            _location = indexLocation;
        }

        private void LoadIndex()
        {
            _searcher = new IndexSearcher(FSDirectory.Open(new DirectoryInfo(_location)), true);
        }

        public void GetAllMoviesWith(string textSearch, int maxCount, out List<string> movies, out List<string> reviews, IList<string> filters = null)
        {
            reviews  = new List<string>();
            movies = new List<string>();

            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var query = ParseQuery(textSearch, filters, analyzer);
                var hits = _searcher.Search(query, 10);

                if (hits == null)
                {
                    Trace.TraceWarning("Search query {0} didn't generate any results", textSearch);
                    return;
                }
                else
                {
                    Trace.TraceInformation("Results for search query {0} : hits {1} max score {2} score docs {3}", textSearch, hits.TotalHits, hits.MaxScore, hits.ScoreDocs);
                    foreach (var doc in hits.ScoreDocs)
                    {
                        GetIdsFromSearchQueries(_searcher.Doc(doc.Doc), movies, reviews);
                    }
                }
            }
            catch (Exception err)
            {
               Trace.TraceError("Get all movies failed with exception {0}", err);
                throw;
            }

        }

        private void GetIdsFromSearchQueries(Document doc, IList<string> movieIds, IList<string> reviewIds)
        {
            var id = doc.Get(Constants.Constants.Field_Id);
            var type = doc.Get(Constants.Constants.Field_EntityType);

            if (string.Compare(type, Constants.Constants.Field_EntityType_Movie, true) == 0)
            {
                movieIds.Add(id);
            }
            else if (string.Compare(type, Constants.Constants.Field_EntityType_Reviews, true) == 0)
            {
                reviewIds.Add(id);
            }
        }

        Query ParseQuery(string query, IList<string> filters, Analyzer analyzer)
        {
            query = query.Trim();

            if (filters == null)
            {
                var field = Constants.Constants.Field_Actors;
                if (string.IsNullOrEmpty(field))
                {
                    field = Constants.Constants.Field_Directors;
                }
                var parser = new QueryParser(Version.LUCENE_30, field , analyzer);
                return parser.Parse(query);
            }
            else
            {
                var parser = new MultiFieldQueryParser(Version.LUCENE_30, filters.ToArray(), analyzer);
                return parser.Parse(query);
            }
        }
         
    }
}
