using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;
using System.Configuration;

namespace LuceneSearchLibrarby
{
    public class LuceneSearch
    {
        /// <summary>
        /// We have added Lucene search index directory handler to make our LuceneSearch class ready to have search methods added.
        /// </summary>        
        private static string _luceneDir = Path.Combine(ConfigurationManager.AppSettings["ImagePath"], "lucene_index");
        private static FSDirectory _directoryTemp;

        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                {
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                }

                if (IndexWriter.IsLocked(_directoryTemp))
                {
                    IndexWriter.Unlock(_directoryTemp);
                }

                var lockFilePath = Path.Combine(_luceneDir, "write.lock");

                if (File.Exists(lockFilePath))
                {
                    try
                    {
                        File.Delete(lockFilePath);
                    }
                    catch (IOException)
                    { }
                }

                return _directoryTemp;
            }
        }

        /// <summary>
        /// This is a private method that creates a single search index entry based on our data, and it will be reused by public methods.
        /// </summary>
        /// <param name="movieSearchData">Object of MovieSearchData type</param>
        /// <param name="writer"></param>
        private static void _addToLuceneIndex(MovieSearchData movieSearchData, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", movieSearchData.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", movieSearchData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Title", movieSearchData.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("UniqueName", movieSearchData.UniqueName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("TitleImageURL", movieSearchData.TitleImageURL, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Type", movieSearchData.Type, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Link", movieSearchData.Link, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", movieSearchData.Description, Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        /// <summary>
        /// method that will use _addToLuceneIndex() in order to add a list of records to search index: 
        /// </summary>
        /// <param name="movieSearchDatas">List of MovieSearchData</param>
        public static void AddUpdateLuceneIndex(IEnumerable<MovieSearchData> movieSearchDatas)
        {
            // init lucene
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    foreach (var movieSearchData in movieSearchDatas)
                    {
                        _addToLuceneIndex(movieSearchData, writer);
                    }

                    // close handles
                    analyzer.Close();
                }
            }
        }

        /// <summary>
        /// This is a public method that will add a single record to search index:
        /// </summary>
        /// <param name="movieSearchData"></param>
        public static void AddUpdateLuceneIndex(MovieSearchData movieSearchData)
        {
            AddUpdateLuceneIndex(new MovieSearchData[] { movieSearchData });
        }

        /// <summary>
        /// method to remove single record from Lucene search index by record's Id field: 
        /// </summary>
        /// <param name="record_id">record id</param>
        public static void ClearLuceneIndexRecord(int record_id)
        {
            // init lucene
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    // close handles
                    analyzer.Close();
                }
            }
        }

        /// <summary>
        /// This method simply removes the whole Lucene search index via a method built into Lucene IndexWriter
        /// </summary>
        /// <returns></returns>
        public static bool ClearLuceneIndex()
        {
            try
            {
                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {
                    //var analyzer = new WhitespaceAnalyzer();
                    using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        // remove older index entries
                        writer.DeleteAll();

                        // close handles
                        analyzer.Close();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// speed up searches, especially if index is getting bigger
        /// </summary>
        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            //var analyzer = new WhitespaceAnalyzer();
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        private static MovieSearchData _mapLuceneDocumentToData(Document doc)
        {
            return new MovieSearchData
            {
                Id = doc.Get("Id"),
                Title = doc.Get("Title"),
                UniqueName = doc.Get("UniqueName"),
                TitleImageURL = doc.Get("TitleImageURL"),
                Type = doc.Get("Type"),
                Link = doc.Get("Link"),
                Description = doc.Get("Description")
            };
        }

        private static IEnumerable<MovieSearchData> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData);
        }

        private static IEnumerable<MovieSearchData> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc)));
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query = null;

            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }

        private static IEnumerable<MovieSearchData> _search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", string.Empty).Replace("?", string.Empty)))
            {
                return Enumerable.Empty<MovieSearchData>();
            }

            // set up lucene searcher
            var results = Enumerable.Empty<MovieSearchData>();
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 10;
                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {
                    //var analyzer = new WhitespaceAnalyzer();

                    // search by single field
                    if (!string.IsNullOrEmpty(searchField))
                    {
                        var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hits_limit).ScoreDocs;
                        results = _mapLuceneToDataList(hits, searcher);
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        //var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "Id", "Title", "UniqueName", "TitleImageURL", "Type", "Link", "Description" }, analyzer);
                        var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "Id", "Title", "UniqueName", "Type", "Description" }, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        results = _mapLuceneToDataList(hits, searcher);
                    }

                    analyzer.Close();
                }
            }

            return results;
        }

        public static IEnumerable<MovieSearchData> Search(string input, string fieldName = "")
        {
            input = input.Trim().Replace("-", " ");
            if (string.IsNullOrEmpty(input))
            {
                return Enumerable.Empty<MovieSearchData>();
            }

            var terms =
                input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x + "*");

            input = string.Join(" ", terms);
            return _search(input, fieldName);
        }

        public static IEnumerable<MovieSearchData> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ?
                Enumerable.Empty<MovieSearchData>() :
                _search(input, fieldName);
        }

        public static IEnumerable<MovieSearchData> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any())
            {
                return Enumerable.Empty<MovieSearchData>();
            }

            // set up lucene searcher
            var docs = new List<Document>();
            using (var searcher = new IndexSearcher(_directory, false))
            {
                using (var reader = IndexReader.Open(_directory, false))
                {
                    var term = reader.TermDocs();
                    while (term.Next())
                    {
                        docs.Add(searcher.Doc(term.Doc));
                    }
                }
            }

            return _mapLuceneToDataList(docs);
        }
    }
}
