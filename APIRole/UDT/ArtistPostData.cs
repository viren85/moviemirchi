
namespace CloudMovie.APIRole.UDT
{
    using DataStoreLib.Models;

    public class ArtistPostData
    {
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string UniqueName { get; set; }
        public string Bio { get; set; }
        public string Born { get; set; }
        public string MovieList { get; set; }
        public string Popularity { get; set; }
        public string Posters { get; set; }
        public string MyScore { get; set; }
        public string TwitterHandle { get; set; }
        public string JsonString { get; set; }
        public string ArtistNickName { get; set; }
        public string Age { get; set; }
        public string FamilyRelation { get; set; }
        public string DateOfBirth { get; set; }
        public string BornCity { get; set; }
        public string ZodiacSign { get; set; }
        public string Hobbies { get; set; }
        public string EducationDetails { get; set; }
        public string SocialActivities { get; set; }
        public string DebutFilms { get; set; }
        public string RememberForMovies { get; set; }
        public string Awards { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string Summary { get; set; }

        public ArtistEntity GetArtistEntity()
        {
            ArtistEntity artistEntity = new ArtistEntity();

            artistEntity.ArtistId = this.ArtistId;
            artistEntity.ArtistName = this.ArtistName;
            artistEntity.UniqueName = this.UniqueName;
            artistEntity.Bio = this.Bio;
            artistEntity.Born = this.Born;
            artistEntity.MovieList = this.MovieList;
            artistEntity.Popularity = this.Popularity;
            artistEntity.Posters = this.Posters;
            artistEntity.MyScore = this.MyScore;
            artistEntity.JsonString = this.JsonString;
            artistEntity.TwitterHandle = this.TwitterHandle;
            artistEntity.ArtistNickName = this.ArtistNickName;
            artistEntity.Age = this.Age;
            artistEntity.FamilyRelation = this.FamilyRelation;
            artistEntity.DateOfBirth = this.DateOfBirth;
            artistEntity.BornCity = this.BornCity;
            artistEntity.ZodiacSign = this.ZodiacSign;
            artistEntity.Hobbies = this.Hobbies;
            artistEntity.EducationDetails = this.EducationDetails;
            artistEntity.SocialActivities = this.SocialActivities;
            artistEntity.DebutFilms = this.DebutFilms;
            artistEntity.RememberForMovies = this.RememberForMovies;
            artistEntity.Awards = this.Awards;
            artistEntity.FacebookURL = this.FacebookURL;
            artistEntity.InstagramURL = this.InstagramURL;
            artistEntity.Summary = this.Summary;

            return artistEntity;
        }
    }
}