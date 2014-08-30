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
        public const string NewsEntities = "News";
        public const string TwitterEntities = "Twitter";
        public const string ArtistEntity = "Artist";
        public const string ReviewEntity = "Reviews";
        public const string ReviewerMoviesEntity = "ReviewMovies";
        #endregion

        #region JSON
        public const string PopularTagsJson = "PopularTags";
        public const string UserFeedback = "UserFeedbackPopularElements";
        #endregion

        #region Json for cetains keys
        public const string MovieInfoJson = "MovieInfoJson_";
        #endregion
    }
}
