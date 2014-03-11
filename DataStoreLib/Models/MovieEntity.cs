using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class MovieEntity : TableEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";

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


        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
                                       Microsoft.WindowsAzure.Storage.OperationContext operationContext)
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
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
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

            return dict;
        }
        #endregion
        public MovieEntity()
            : base(PARTITION_KEY, "")
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
                                                    string year, string uniqueName)
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

            return entity;
        }

        #region AccessMethods
        public List<string> GetAltNames()
        {
            return Utils.utils.GetListFromCommaSeparatedString(AltNames);
        }

        public List<string> GetActors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Posters);
        }

        public List<string> GetDirectors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Ratings);
        }

        public List<string> GetProducers()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Synopsis);
        }

        public List<string> GetMusicDirectors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Casts);
        }

        public List<string> GetReviewIds()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Stats);
        }

        public void SetAltNames(List<string> list)
        {// todo :: 
            //AltNames = Utils.utils.GetListFromCommaSeparatedString(AltNames);
        }

        public void SetActors(List<string> list)
        {
            Posters = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetDirectors(List<string> list)
        {
            Ratings = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetMusicDirectors(List<string> list)
        {
            Casts = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewIds(List<string> list)
        {
            Stats = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        #endregion
    }
}
