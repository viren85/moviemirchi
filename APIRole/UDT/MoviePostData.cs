
namespace CloudMovie.APIRole.UDT
{
    using DataStoreLib.Models;

    public class MoviePostData
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string AltNames { get; set; }
        public string Posters { get; set; }
        public string Ratings { get; set; }
        public string Synopsis { get; set; }
        public string Casts { get; set; }
        public string Stats { get; set; }
        public string Songs { get; set; }
        public string Trailers { get; set; }
        public string Pictures { get; set; }
        public string Genre { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string UniqueName { get; set; }
        public string State { get; set; }
        public string MyScore { get; set; }
        public string JsonString { get; set; }
        public string Popularity { get; set; }

        public MovieEntity GetMovieEntity()
        {
            // TODO: Add error/edge-case handling as appropriate
            MovieEntity movie = new MovieEntity();
            movie.MovieId = this.MovieId;
            movie.Name = this.Name;
            movie.AltNames = this.AltNames;
            movie.Posters = this.Posters;
            movie.Ratings = this.Ratings;
            movie.Synopsis = this.Synopsis;
            movie.Casts = this.Casts;
            movie.Stats = this.Stats ?? string.Empty;
            movie.Songs = this.Songs;
            movie.Trailers = this.Trailers;
            movie.Pictures = this.Pictures;
            movie.Genre = this.Genre;
            movie.Month = this.Month;
            movie.Year = this.Year;
            movie.UniqueName = this.UniqueName;
            movie.State = this.State;
            movie.MyScore = this.MyScore;
            movie.JsonString = this.JsonString;
            movie.Popularity = this.Popularity;
            return movie;
        }
    }
}