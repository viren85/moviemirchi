
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class BuildLuceneController : BaseController
    {
        protected override string ProcessRequest()
        {
            // Movies
            // 1. Get all movies
            // 2. Loop through movies and add it to index
            // 3. Set default poster - as selected in admin panel
            BuildMovieIndex();

            // Artists
            // 1. Get all artists
            // 2. Loop through artists and add it to index
            // 3. Set default poster - as selected in admin panel

            // Critics
            // 1. Get all critics
            // 2. Loop through critics and add it to index
            // 3. Set default poster

            // Genre

            return string.Empty;
        }

        private void BuildMovieIndex()
        {
            TableManager tblMgr = new TableManager();
            JavaScriptSerializer json = new JavaScriptSerializer();

            IDictionary<string, MovieEntity> movies = tblMgr.GetAllMovies();

            string posterUrl = string.Empty;

            foreach (MovieEntity movie in movies.Values)
            {
                List<String> posters = json.Deserialize(movie.Posters, typeof(List<String>)) as List<String>;
                List<APIRole.UDT.Cast> casts = json.Deserialize(movie.Casts, typeof(List<APIRole.UDT.Cast>)) as List<APIRole.UDT.Cast>;

                List<string> actors = new List<string>();
                List<string> critics = new List<string>();

                MovieSearchData movieSearchIndex = new MovieSearchData();
                IDictionary<string, ReviewEntity> reviews = tblMgr.GetReviewsByMovieId(movie.MovieId);

                if (posters != null && posters.Count > 0)
                {
                    posterUrl = posters[posters.Count - 1];
                }

                if (reviews != null)
                {
                    foreach (ReviewEntity review in reviews.Values)
                    {
                        if (!string.IsNullOrEmpty(review.ReviewerName))
                            critics.Add(review.ReviewerName);
                    }
                }

                if (casts != null)
                {
                    foreach (var actor in casts)
                    {
                        // actor, director, music, producer
                        string role = actor.role.ToLower();
                        string characterName = string.IsNullOrEmpty(actor.charactername) ? string.Empty : actor.charactername;

                        // Check if artist is already present in the list for some other role.
                        // If yes, skip it. Also if the actor name is missing then skip the artist
                        if (actors.Contains(actor.name) || string.IsNullOrEmpty(actor.name) || actor.name == "null")
                            continue;

                        // If we want to showcase main artists and not all, keep the following switch... case.
                        switch (role)
                        {
                            case "actor":
                                actors.Add(actor.name);
                                break;
                            case "producer":
                                // some times producer are listed as line producer etc. 
                                // We are not interested in those artists as of now?! Hence skipping it
                                if (characterName == role)
                                {
                                    actors.Add(actor.name);
                                }
                                break;
                            case "music":
                            case "director":
                                // Main music director and movie director does not have associated character name.
                                // Where as other side directors have associated character name as associate director, assitant director.
                                // Skipping such cases.
                                if (string.IsNullOrEmpty(characterName))
                                {
                                    actors.Add(actor.name);
                                }
                                break;
                        }

                    }
                }

                movieSearchIndex.Id = movie.RowKey;
                movieSearchIndex.Title = movie.Name;
                movieSearchIndex.Type = movie.Genre;

                // Selected poster url 
                movieSearchIndex.TitleImageURL = posterUrl;

                movieSearchIndex.UniqueName = movie.UniqueName;
                movieSearchIndex.Description = json.Serialize(actors);
                movieSearchIndex.Critics = json.Serialize(critics);
                movieSearchIndex.Link = movie.UniqueName;
                LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
            }
        }

        private void BuildArtistIndex()
        {

        }

        private void BuildCriticsIndex()
        {

        }

        private void BuildGenreIndex() { }
    }
}
