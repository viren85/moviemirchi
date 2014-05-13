
namespace DataStoreLib.Constants
{
    public static class Constants
    {
        #region API Messages
        public const string API_EXC_MOVIE_NAME_NOT_EXIST = "Movie name is not supplied.";
        public const string API_EXC_REVIEWERNAME_NOT_EXIST = "Reviewer name is not supplied.";
        public const string API_EXC_SEARCH_TEXT_NOT_EXIST = "Search text is not supplied.";
        #endregion

        #region User Messages
        public const string UM_WHILE_GETTING_MOVIE = "Error occured while getting movie informatin by movie name.";
        public const string UM_WHILE_GETTING_CURRENT_MOVIES = "Error occured while getting current movies.";
        public const string UM_WHILE_GETTING_ARTIST_MOVIES = "Error occured while getting movies for Artists.";
        public const string UM_WHILE_GETTING_REVIEWER_INFO = "Error occured while getting reviewer infromation with their reivews";
        public const string UM_WHILE_SEARCHING_MOVIES = "Error occured while searching movies";
        public const string UM_WHILE_SEARCHING_MOVIES_TRAILER = "Error occured while getting movie's trailers";
        public const string UM_WHILE_SEARCHING_MOVIES_SONGS = "Error occured while getting movie's songs";
        public const string UM_WHILE_SEARCHING_MOVIES_POSTER = "Error occured while getting movie's posters/pictures";
        public const string UM_WHILE_SEARCHING_SONGS = "Error occured while searching songs";
        public const string UM_WHILE_GETTING_TWEETS = "Error occured while getting Tweets";
        #endregion
    }
}
