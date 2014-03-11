using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Storage
{
    public interface IStore
    {
        IDictionary<string, MovieEntity> GetMoviesByid(List<string> id);
        IDictionary<string, ReviewEntity> GetReviewsById(List<string> id);

        /* added a new method for getting all movies*/
        IDictionary<string, MovieEntity> GetAllMovies();
        IDictionary<string, MovieEntity> GetMoviesByUniqueName(string name);
        IDictionary<string, ReviewEntity> GetReviewsByMovieId(string movieId);
        IDictionary<string, ReviewEntity> GetReviewsByReviewer(string reviewerName);
        /* end */


        IDictionary<MovieEntity, bool> UpdateMoviesById(List<MovieEntity> movies);
        IDictionary<ReviewEntity, bool> UpdateReviewsById(List<ReviewEntity> reviews);

        IDictionary<string, AuthenticationEntity> GetUsersByName(string userName);
        IDictionary<AuthenticationEntity, bool> UpdateUsersById(List<AuthenticationEntity> user);

        IDictionary<string, AuthenticationEntity> GetAllUser();
    }

    public static class IStoreHelpers
    {
        public static MovieEntity GetMovieById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetMoviesByid(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static MovieEntity GetMovieByUniqueName(this IStore store, string name)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name));

            var retList = store.GetMoviesByUniqueName(name);

            Debug.Assert(retList.Count == 1);
            if (retList.Count > 0)
                return retList[retList.Keys.FirstOrDefault()];
            else
                return null;
        }

        public static ReviewEntity GetReviewById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetReviewsById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdateMovieById(this IStore store, MovieEntity movie)
        {
            Debug.Assert( movie != null );
            var list = new List<MovieEntity> { movie };
            var retList = store.UpdateMoviesById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdateReviewById(this IStore store, ReviewEntity review)
        {
            Debug.Assert(review != null);
            var list = new List<ReviewEntity> { review };
            var retList = store.UpdateReviewsById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        /* Method added */
        /// <summary>
        /// get list of current running (in theaters) movies
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static List<MovieEntity> GetCurrentMovies(this IStore store)
        {
            var retList = store.GetAllMovies();
            //Debug.Assert(retList.Count == 1);

            List<MovieEntity> currentMovies = new List<MovieEntity>();

            foreach (var currentMovie in retList.Values)
            {
                string currentMonth = DateTime.Now.ToString("MMMM");
                string year = DateTime.Now.Year.ToString();

                if (currentMovie.Month == currentMonth && currentMovie.Year == year)
                {
                    currentMovies.Add(currentMovie);
                }
            }

            return currentMovies;
        }

        /// <summary>
        /// search movies and return a list of movies according to search text
        /// </summary>
        /// <param name="store">IStore interface type object</param>
        /// <param name="searchText">search text like name of the movie etc</param>
        /// <returns></returns>
        public static List<MovieEntity> SearchMovies(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            
            List<MovieEntity> currentMovies = new List<MovieEntity>();

            foreach (var currentMovie in retList.Values)
            {
                if (currentMovie.Name.ToLower().Contains(searchText.ToLower()))
                {
                    currentMovies.Add(currentMovie);
                }
            }

            return currentMovies;
        }

        /// <summary>
        /// Return a list of movies order by name
        /// </summary>
        /// <param name="store">IStore interface type object</param>        
        /// <returns></returns>
        public static List<MovieEntity> GetSortedMoviesByName(this IStore store)
        {
            var retList = store.GetAllMovies();

            //Debug.Assert(retList.Count == 1);

            List<MovieEntity> currentMovies = new List<MovieEntity>();

            if (retList != null && retList.Values != null)
            {
                currentMovies = (List<MovieEntity>)retList.Values.OrderBy(m => m.Name).ToList();
            }

            return currentMovies;
        }

        /// <summary>
        /// return the list of song in sorted order
        /// </summary>
        /// <param name="store"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static List<MovieEntity> SearchSongs(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> currentSongs = new List<MovieEntity>();

            foreach (var currentSong in retList.Values)
            {
                if (currentSong.Songs.Contains(searchText.ToLower()))
                {
                    currentSongs.Add(currentSong);
                }
            }
            
            return currentSongs;
        }

        public static List<MovieEntity> SearchMoviesByActor(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> actors = new List<MovieEntity>();

            foreach (var actor in retList.Values)
            {
                if (actor.Casts.ToString().Contains(searchText.ToLower()))
                {
                    actors.Add(actor);
                }
            }
            
            return actors;
        }

        public static List<MovieEntity> SearchTitle(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> titles = new List<MovieEntity>();

            foreach (var title in retList.Values)
            {
                if (title.Name.Contains(searchText.ToLower()))
                {
                    titles.Add(title);
                }
            }
            
            return titles;
        }

        public static List<MovieEntity> SearchTrailer(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> traileres = new List<MovieEntity>();

            foreach (var trailer in retList.Values)
            {
                if (trailer.Trailers.Contains(searchText.ToLower()))
                {
                    traileres.Add(trailer);
                }
            }
            
            return traileres;
        }

        public static List<MovieEntity> SearchCharacter(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> characters = new List<MovieEntity>();

            foreach (var character in retList.Values)
            {
                if (character.Casts.Contains(searchText.ToLower()))
                {
                    characters.Add(character);
                }
            }
            
            return characters;
        }

        public static List<AuthenticationEntity> SearchUser(this IStore store, string searchText)
        {
            var retList = store.GetAllUser();
            Debug.Assert(retList.Count == 1);

            List<AuthenticationEntity> users = new List<AuthenticationEntity>();

            foreach (var user in retList.Values)
            {
                if (user.UserName.Contains(searchText.ToLower()))
                {
                    users.Add(user);
                }
            }

            users.Equals(searchText);

            //currentSongs.Sort();
            return users;
        }

        public static AuthenticationEntity GetUserByName(this IStore store, string userName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(userName));

            var retList = store.GetUsersByName(userName);

            Debug.Assert(retList.Count == 1);
            if (retList.Count > 0)
                return retList[retList.Keys.FirstOrDefault()];
            else
                return null;
        }

        public static bool UpdateUserById(this IStore store, AuthenticationEntity user)
        {
            Debug.Assert(user != null);
            var list = new List<AuthenticationEntity> { user };
            var retList = store.UpdateUsersById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }
    }
}
