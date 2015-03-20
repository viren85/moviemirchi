
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class MovieEntity : TableStorageEntity
    {

        #region table members
        public const string PARTITION_KEY = "CloudMovie";

        public string MovieId { get; set; }
        public string Name { get; set; }
        public string AltNames { get; set; }
        public string Posters { get; set; }
        public string Rating { get; set; }
        public string Synopsis { get; set; }
        public string Cast { get; set; }
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
        public string TwitterHandle { get; set; }

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
                return new DateTime(1900, 1, 1);
            }
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
            Rating = entity.Rating;
            Synopsis = entity.Synopsis;
            Cast = entity.Cast;
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
            TwitterHandle = entity.TwitterHandle;
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
                                                    string popularity,
                                                    string twitterHandle
                                                    )
        {
            var movieId = Guid.NewGuid().ToString();
            var entity = new MovieEntity(movieId);
            entity.MovieId = movieId;
            entity.Name = name;
            entity.Posters = posters;
            entity.Rating = rating;
            entity.Synopsis = synopsis;
            entity.Cast = cast;
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
            entity.TwitterHandle = twitterHandle;

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

            foreach (Cast c in casts)
            {
                if (!string.IsNullOrEmpty(c.name))
                    castName.Add(c.name);
            }

            return castName;
        }

        public IEnumerable<string> GetDirectors()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Rating);
        }

        public IEnumerable<string> GetProducers()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Synopsis);
        }

        public IEnumerable<string> GetMusicDirectors()
        {
            List<string> musicDirectors = new List<string>();
            IEnumerable<string> music = Utils.Utils.GetListFromCommaSeparatedString(Cast);
            foreach (string str in music)
            {
                if (str.Contains("music"))
                {
                    musicDirectors.Add(str);
                }
            }

            return musicDirectors;
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
            Rating = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetMusicDirectors(IEnumerable<string> list)
        {
            Cast = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewIds(IEnumerable<string> list)
        {
            Stats = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        #endregion
    }
}
