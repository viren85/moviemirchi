
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;    
    using System;
    using System.Collections.Generic;

    public class MovieEntity : TableEntity
    {

        #region table members
        public const string PARTITION_KEY = "CloudMovie";

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
        // For future use
        public string JsonString { get; set; }
        public string Popularity { get; set; }

        public DateTime PublishDate
        {
            get
            {
                var strDate = this.Month;
                if (!string.IsNullOrWhiteSpace(strDate))
                {
                    strDate = strDate.Split('(')[0];
                    DateTime date;
                    if (DateTime.TryParse(strDate, out date))
                    {
                        return date.Date;
                    }
                }
                return new DateTime(0, 0, 0);
            }
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            MovieId = ReadString(properties, "MovieId");
            Name = ReadString(properties, "Name");
            AltNames = ReadString(properties, "AltNames");
            Posters = ReadString(properties, "Posters");
            Ratings = ReadString(properties, "Rating");
            Synopsis = ReadString(properties, "Synopsis");
            Casts = ReadString(properties, "Cast");
            Stats = ReadString(properties, "Stats");
            Songs = ReadString(properties, "Songs");
            Trailers = ReadString(properties, "Trailers");
            Pictures = ReadString(properties, "Pictures");
            Genre = ReadString(properties, "Genre");
            Month = ReadString(properties, "Month");
            Year = ReadString(properties, "Year");
            UniqueName = ReadString(properties, "UniqueName");
            State = ReadString(properties, "State");
            MyScore = ReadString(properties, "MyScore");
            JsonString = ReadString(properties, "JsonString");
            Popularity = ReadString(properties, "Popularity");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "MovieId", MovieId);
            WriteString(dict, "Name", Name);
            WriteString(dict, "AltNames", AltNames);
            WriteString(dict, "Posters", Posters);
            WriteString(dict, "Rating", Ratings);
            WriteString(dict, "Synopsis", Synopsis);
            WriteString(dict, "Cast", Casts);
            WriteString(dict, "Stats", Stats);
            WriteString(dict, "Songs", Songs);
            WriteString(dict, "Trailers", Trailers);
            WriteString(dict, "Pictures", Pictures);
            WriteString(dict, "Month", Month);
            WriteString(dict, "Year", Year);
            WriteString(dict, "Genre", Genre);
            WriteString(dict, "UniqueName", UniqueName);
            WriteString(dict, "State", State);
            WriteString(dict, "MyScore", MyScore);
            WriteString(dict, "JsonString", JsonString);
            WriteString(dict, "Popularity", Popularity);

            return dict;
        }
        #endregion
        public MovieEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public MovieEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public MovieEntity(MovieEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            MovieId = entity.MovieId;
            Name = entity.Name;
            AltNames = entity.AltNames;
            Posters = entity.Posters;
            Ratings = entity.Ratings;
            Synopsis = entity.Synopsis;
            Casts = entity.Casts;
            Stats = entity.Stats;
            Songs = entity.Songs;
            Trailers = entity.Trailers;
            Pictures = entity.Pictures;
            Genre = entity.Genre;
            Month = entity.Month;
            Year = entity.Year;
            UniqueName = entity.UniqueName;
            State = entity.State;
            MyScore = entity.MyScore;
            JsonString = entity.JsonString;
            Popularity = entity.Popularity;
        }

        public override string GetKey()
        {
            return this.MovieId;
        }

        public static MovieEntity CreateMovieEntity(string name,
                                                    string posters,
                                                    string rating,
                                                    string synopsis,
                                                    string cast,
                                                    string stats,
                                                    string songs,
                                                    string trailers,
                                                    string pictures,
                                                    string genre,
                                                    string month,
                                                    string year,
                                                    string uniqueName,
                                                    string state,
                                                    string myScore,
                                                    string jsonString,
                                                    string popularity
                                                    )
        {
            var movieId = Guid.NewGuid().ToString();
            var entity = new MovieEntity(movieId);
            entity.MovieId = movieId;
            entity.Name = name;
            entity.Posters = posters;
            entity.Ratings = rating;
            entity.Synopsis = synopsis;
            entity.Casts = cast;
            entity.Stats = stats;
            entity.Songs = songs;
            entity.Trailers = trailers;
            entity.Pictures = pictures;
            entity.Genre = genre;
            entity.Month = month;
            entity.Year = year;
            entity.UniqueName = uniqueName;
            entity.State = state;
            entity.MyScore = myScore;
            entity.JsonString = jsonString;
            entity.Popularity = popularity;

            return entity;
        }

        #region AccessMethods
        public IEnumerable<string> GetAltNames()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(AltNames);
        }

        public IEnumerable<string> GetActors()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Posters);
        }

        public IEnumerable<string> GetActors(List<Cast> casts)
        {
            //return Utils.Utils.GetListFromCommaSeparatedString(Posters);

            List<string> castName = new List<string>();

            foreach (Cast c in casts) {
                castName.Add(c.name);
            }

            return castName;
        }

        public IEnumerable<string> GetDirectors()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Ratings);
        }

        public IEnumerable<string> GetProducers()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Synopsis);
        }

        public IEnumerable<string> GetMusicDirectors()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Casts);
        }

        public IEnumerable<string> GetReviewIds()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Stats);
        }

        public void SetAltNames(IEnumerable<string> list)
        {// todo :: 
            //AltNames = Utils.utils.GetListFromCommaSeparatedString(AltNames);
        }

        public void SetActors(IEnumerable<string> list)
        {
            Posters = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetDirectors(IEnumerable<string> list)
        {
            Ratings = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetMusicDirectors(IEnumerable<string> list)
        {
            Casts = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewIds(IEnumerable<string> list)
        {
            Stats = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        #endregion
    }
}
