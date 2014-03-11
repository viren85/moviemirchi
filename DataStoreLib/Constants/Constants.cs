using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Constants
{
    public class Constants
    {
        #region API Messages
        public static readonly string API_EXC_MOVIE_NAME_NOT_EXIST = "Movie name is not supplied.";
        public static readonly string API_EXC_REVIEWERNAME_NOT_EXIST = "Reviewer name is not supplied.";
        public static readonly string API_EXC_SEARCH_TEXT_NOT_EXIST = "Search text is not supplied.";
        #endregion

        #region User Messages
        public static readonly string UM_WHILE_GETTING_MOVIE = "Error occured while getting movie informatin by movie name.";
        public static readonly string UM_WHILE_GETTING_CURRENT_MOVIES = "Error occured while getting current movies.";
        public static readonly string UM_WHILE_GETTING_REVIEWER_INFO = "Error occured while getting reviewer infromation with their reivews";
        public static readonly string UM_WHILE_SEARCHING_MOVIES = "Error occured while searching movies";
        public static readonly string UM_WHILE_SEARCHING_MOVIES_TRAILER = "Error occured while getting movie's trailers";
        public static readonly string UM_WHILE_SEARCHING_MOVIES_SONGS = "Error occured while getting movie's songs";
        public static readonly string UM_WHILE_SEARCHING_MOVIES_POSTER = "Error occured while getting movie's posters/pictures";
        public static readonly string UM_WHILE_SEARCHING_SONGS = "Error occured while searching songs";
        #endregion
    }
}
