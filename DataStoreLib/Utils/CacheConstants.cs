using System;

namespace DataStoreLib.Utils
{
    public static class CacheConstants
    {
        #region Entity
        public const string AllMovieEntities = "AllMovies";
        public const string UpcomingMovieEntities = "UpcomingMovies";
        public const string NowPlayingMovieEntities = "NowPlayingMovies";
        public const string AllMovieEntitiesSortedByName = "AllMoviesSortedByName";
        #endregion

        #region JSON
        public const string PopularTagsJson = "PopularTags";
        #endregion

        #region Json for cetains keys
        public const string MovieInfoJson = "MovieInfoJson_";
        #endregion
    }
}
