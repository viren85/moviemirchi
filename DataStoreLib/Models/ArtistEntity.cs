
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class ArtistEntity : TableStorageEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string UniqueName { get; set; }
        public string Bio { get; set; }
        public string Born { get; set; }
        public string MovieList { get; set; }
        public string Popularity { get; set; }
        public string Posters { get; set; }
        public string MyScore { get; set; }
        public string TwitterHandle { get; set; } //this is part of social media

        // For future use
        public string JsonString { get; set; }

        //added by vasim for extra info
        public string ArtistNickName { get; set; }
        public string Age { get; set; }
        public string FamilyRelation { get; set; } //Shall be JSON(Multiple entry)
        public string DateOfBirth { get; set; }
        public string BornCity { get; set; }
        public string ZodiacSign { get; set; }
        public string Hobbies { get; set; }
        public string EducationDetails { get; set; } //Shall be json(Multiple entry)
        public string SocialActivities { get; set; }
        public string DebutFilms { get; set; } //Shall be json(Multiple entry)
        public string RememberForMovies { get; set; } //Shall be json(Multiple entry)
        public string Awards { get; set; } //Shall be json(Multiple entry)
        public string FacebookURL { get; set; } //this is part of social media
        public string InstagramURL { get; set; } //this is part of social media
        public string Summary { get; set; }
        //end
        #endregion

        public ArtistEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public ArtistEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
            ArtistId = rowKey;
        }

        public ArtistEntity(ArtistEntity artist)
            : base(artist.PartitionKey, artist.RowKey)
        {

        }

        public override string GetKey()
        {
            return this.RowKey;
        }

        public static ArtistEntity CreateArtistEntity(string artistName, string uniqueName, string bio, string born, string movieList, string popularity,
            string posters, string myScore, string jsonString, string twitterHandle, string artistNickName, string age, string familyRelation, string dateOfBirth,
            string bornCity, string zodiacSign, string hobbies, string educationDetails, string socialActivities, string debutFilms, string rememberMovie, string awards,
            string facebookUrl, string instagramUrl, string summary)
        {
            var artistId = Guid.NewGuid().ToString();
            var artistEntity = new ArtistEntity(uniqueName.ToLower());
            artistEntity.ArtistId = artistId;
            artistEntity.ArtistName = artistName;
            artistEntity.UniqueName = uniqueName;
            artistEntity.Bio = bio;
            artistEntity.Born = born;
            artistEntity.MovieList = movieList;
            artistEntity.Popularity = popularity;
            artistEntity.Posters = posters;
            artistEntity.MyScore = myScore;
            artistEntity.JsonString = jsonString;
            artistEntity.TwitterHandle = twitterHandle;

            //added by vasim
            artistEntity.ArtistNickName = artistNickName;
            artistEntity.Age = age;
            artistEntity.FamilyRelation = familyRelation;
            artistEntity.DateOfBirth = dateOfBirth;
            artistEntity.BornCity = bornCity;
            artistEntity.ZodiacSign = zodiacSign;
            artistEntity.Hobbies = hobbies;
            artistEntity.EducationDetails = educationDetails;
            artistEntity.SocialActivities = socialActivities;
            artistEntity.DebutFilms = debutFilms;
            artistEntity.RememberForMovies = rememberMovie;
            artistEntity.Awards = awards;
            artistEntity.FacebookURL = facebookUrl;
            artistEntity.InstagramURL = instagramUrl;
            artistEntity.Summary = summary;
            //end

            return artistEntity;
        }
    }
}
