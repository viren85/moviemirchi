
namespace SearchLib.Search
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    public class SearchManager
    {
        private static SearchManager _searchManager;
        private static object lockObj = new object();

        protected static SearchManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_searchManager == null)
                    {
                        _searchManager = new SearchManager();
                    }
                }

                return _searchManager;
            }
        }

        protected object readyLock = new object();
        protected enum State { Error, Init, Ready, None };
        protected State objectState = State.None;
        protected IndexQuery index = null;

        [Serializable]
        public class NotReadyException : Exception
        {
            public NotReadyException()
                : base("The indexer is still reading the index. Try querying it later")
            { }
        }

        protected void CheckIfReady()
        {
            lock (readyLock)
            {
                if (objectState == State.Ready)
                {
                    return;
                }
                else if (objectState == State.Error)
                {
                    throw new Exception("The indexer has encountered an error. Please report and or check logs");
                }
                else if (objectState == State.Init)
                {
                    throw new NotReadyException();
                }
                else
                {
                    objectState = State.Init;
                    Thread t = new Thread(new ThreadStart(begin));
                    t.Start();

                    throw new NotReadyException();
                }

            }
        }

        public void begin()
        {
            try
            {
                index = IndexQuery.GetIndexReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexer"));

                lock (readyLock)
                {
                    objectState = State.Ready;
                }
            }
            catch (Exception err)
            {
                Trace.TraceError("Error while loading the index {0}", err);
            }

            return;
        }

        public void GetAllMoviesWith(string textSearch, int maxCount, out List<string> movies, out List<string> reviews, IList<string> filters = null)
        {
            CheckIfReady();

            Debug.Assert(index != null);

            index.GetAllMoviesWith(textSearch, maxCount, out movies, out reviews, filters);
        }

        public void GetMoviesWithname(string name, out List<string> movies)
        {
            List<string> reviews = null;
            GetAllMoviesWith(name, 100, out movies, out reviews, Constants.Constants.MovieNameFilterList);
        }

        public void AddMovieToIndex(string movieName, List<string> altNamesForMovies, string movieId)
        {
            CheckIfReady();

            throw new NotImplementedException();
        }
    }
}
