
namespace LuceneSearchLibrarby
{
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using Version = Lucene.Net.Util.Version;

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
                    File.Delete(lockFilePath);
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
            doc.Add(new Field("Critics", movieSearchData.Critics, Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        /// <summary>
        /// method that will use _addToLuceneIndex() in order to add a list of records to search index: 
        /// </summary>
        /// <param name="movieSearchData">List of MovieSearchData</param>
        public static void AddUpdateLuceneIndex(IEnumerable<MovieSearchData> movieSearchData)
        {
            // init lucene
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    foreach (var data in movieSearchData)
                    {
                        _addToLuceneIndex(data, writer);
                    }
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
        public static void ClearLuceneIndexRecord(string record_id, string deleteByColumn)
        {
            // init lucene
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    //var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                    var searchQuery = new TermQuery(new Term(deleteByColumn, record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        /// <summary>
        /// This method simply removes the whole Lucene search index via a method built into Lucene IndexWriter
        /// </summary>
        /// <returns></returns>
        public static bool ClearLuceneIndex()
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    try
                    {
                        // remove older index entries
                        writer.DeleteAll();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// speed up searches, especially if index is getting bigger
        /// </summary>
        public static void Optimize()
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                //var analyzer = new WhitespaceAnalyzer();
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                }
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
                Description = doc.Get("Description"),
                Critics = doc.Get("Critics")
            };
        }

        private static IEnumerable<MovieSearchData> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<MovieSearchData> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
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
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<MovieSearchData>();

            // set up lucene searcher
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
                        var results = _mapLuceneToDataList(hits, searcher);
                        return results;
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        //var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "Id", "Title", "UniqueName", "TitleImageURL", "Type", "Link", "Description" }, analyzer);
                        var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "Id", "Title", "UniqueName", "Type", "Description", "Critics" }, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        return results;
                    }
                }
            }
        }

        public static IEnumerable<MovieSearchData> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<MovieSearchData>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");

            input = string.Join(" ", terms);

            return _search(input, fieldName);
        }

        public static IEnumerable<MovieSearchData> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<MovieSearchData>() : _search(input, fieldName);
        }

        public static IEnumerable<MovieSearchData> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<MovieSearchData>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }
    }
}
