
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class ArtistEntity : TableEntity
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

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            ArtistId = ReadString(properties, "ArtistId");
            ArtistName = ReadString(properties, "ArtistName");
            UniqueName = ReadString(properties, "UniqueName");
            Bio = ReadString(properties, "Bio");
            Born = ReadString(properties, "Born");
            MovieList = ReadString(properties, "MovieList");
            Popularity = ReadString(properties, "Popularity");
            Posters = ReadString(properties, "Posters");
            MyScore = ReadString(properties, "MyScore");
            JsonString = ReadString(properties, "JsonString");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "ArtistId", ArtistId);
            WriteString(dict, "ArtistName", ArtistName);
            WriteString(dict, "UniqueName", UniqueName);
            WriteString(dict, "Bio", Bio);
            WriteString(dict, "Born", Born);
            WriteString(dict, "MovieList", MovieList);
            WriteString(dict, "Popularity", Popularity);
            WriteString(dict, "Posters", Posters);
            WriteString(dict, "MyScore", MyScore);
            WriteString(dict, "JsonString", JsonString);
            return dict;
        }

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
