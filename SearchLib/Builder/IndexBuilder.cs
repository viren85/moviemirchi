using System.Diagnostics;
using System.IO;
using DataStoreLib.Models;
using DataStoreLib.Storage;
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
using Version = Lucene.Net.Util.Version;

namespace SearchLib.Builder
{
    public class IndexBuilder : IIndexer
    {
        protected string _location;
        protected FSDirectory _dirLocation;

        protected IndexBuilder(string location)
        {
            _location = location;
        }

        public static IIndexer CreateIndexer(string location)
        {
            var indexbuilder = new IndexBuilder(location);
            indexbuilder.LoadIndex();

            return indexbuilder;
        }


        protected void LoadIndex()
        {
            var dir = FSDirectory.Open(new DirectoryInfo(_location));
            if (IndexWriter.IsLocked(dir))
            {
                IndexWriter.Unlock(dir);
            }

            var lockFilePath = Path.Combine(_location, "write.lock");
            if (File.Exists(lockFilePath))
            {
                File.Delete(lockFilePath);
            }

            _dirLocation = dir;
        }

        private List<string> GenerateListFromSet(ISet<string> set)
        {
            var list = new List<string>();
            foreach (var item in set)
            {
                list.Add(item);
            }

            return list;
        }

        public void IndexSelectedMovies(ISet<string> movieIds)
        {
            StandardAnalyzer analyzer = null;
            IndexWriter writer = null;
            try
            {
                analyzer = new StandardAnalyzer(Version.LUCENE_30);
                writer = new IndexWriter(_dirLocation, analyzer,
                                             IndexWriter.MaxFieldLength.UNLIMITED);

                var tableManager = new TableManager();

                var movieList = tableManager.GetMoviesByid(GenerateListFromSet(movieIds));

                foreach (var id in movieIds)
                {
                    if (movieList.ContainsKey(id))
                    {
                        Trace.TraceInformation("Adding {0} to the index", id);

                        var movieEntity = movieList[id];

                        // delete entry if exists
                        var searchQuery = new TermQuery(new Term(Constants.Constants.Field_Id, id));
                        writer.DeleteDocuments(searchQuery);

                        // add to index again
                        var doc = new Document();
                        doc.Add(new Field(Constants.Constants.Field_Id, movieEntity.MovieId, Field.Store.YES,
                                          Field.Index.NOT_ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_Name, movieEntity.Name, Field.Store.YES,
                                          Field.Index.NOT_ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_AltNames, movieEntity.AltNames, Field.Store.NO, Field.Index.ANALYZED));

                        doc.Add(new Field(Constants.Constants.Field_Actors, movieEntity.Posters, Field.Store.NO,
                                          Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_Directors, movieEntity.Ratings, Field.Store.YES,
                                          Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_MusicDirectors, movieEntity.Casts,
                                          Field.Store.YES, Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_Name, movieEntity.Name, Field.Store.YES,
                                          Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_Producers, movieEntity.Synopsis, Field.Store.YES,
                                          Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_MovieSynopsis, movieEntity.Synopsis, Field.Store.YES,
                                          Field.Index.ANALYZED));

                        writer.AddDocument(doc);
                    }
                    else
                    {
                        Trace.TraceWarning("movie {0} not present in db", id);
                    }
                }

            }
            catch (Exception err)
            {
                Trace.TraceError("Failed to build index {0}", err);

            }
            finally
            {
                if (analyzer != null)
                    analyzer.Close();
                if (writer != null)
                    writer.Dispose();
            }
        }

        public void IndexSelectedReviews(ISet<string> reviewIds)
        {
            StandardAnalyzer analyzer = null;
            IndexWriter writer = null;
            try
            {
                analyzer = new StandardAnalyzer(Version.LUCENE_30);
                writer = new IndexWriter(_dirLocation, analyzer,
                                             IndexWriter.MaxFieldLength.UNLIMITED);

                var tableManager = new TableManager();

                var reviewList = tableManager.GetReviewsById(GenerateListFromSet(reviewIds));

                foreach (var id in reviewIds)
                {
                    if (reviewList.ContainsKey(id))
                    {
                        Trace.TraceInformation("Adding {0} to the index", id);

                        var reviewEntity = reviewList[id];

                        // delete entry if exists
                        var searchQuery = new TermQuery(new Term(Constants.Constants.Field_Id, id));
                        writer.DeleteDocuments(searchQuery);

                        // add to index again
                        var doc = new Document();
                        doc.Add(new Field(Constants.Constants.Field_Id, reviewEntity.ReviewId, Field.Store.YES,
                                          Field.Index.NOT_ANALYZED));

                        doc.Add(new Field(Constants.Constants.Field_EntityType, Constants.Constants.Field_EntityType_Reviews, Field.Store.YES,
                                          Field.Index.NOT_ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_ReviewerName, reviewEntity.ReviewerName, Field.Store.YES,
                                          Field.Index.ANALYZED));
                        doc.Add(new Field(Constants.Constants.Field_EntityType_ReviewText, reviewEntity.Review, Field.Store.YES, Field.Index.ANALYZED));
                        
                        writer.AddDocument(doc);
                    }
                    else
                    {
                        Trace.TraceWarning("movie {0} not present in db", id);
                    }
                }

            }
            catch (Exception err)
            {
                Trace.TraceError("Failed to build index {0}", err);

            }
            finally
            {
                if (analyzer != null)
                    analyzer.Close();
                if (writer != null)
                    writer.Dispose();
            }
        }
    }
}
