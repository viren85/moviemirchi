
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

        // For future use
        public string JsonString { get; set; }

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
            return this.ArtistId;
        }

        public static ArtistEntity CreateArtistEntity(string artistName, string uniqueName, string bio, string born, string movieList, string popularity, string posters, string myScore, string jsonString)
        {
            var artistId = Guid.NewGuid().ToString();
            var artistEntity = new ArtistEntity(artistId);
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
            return artistEntity;
        }
    }
}
