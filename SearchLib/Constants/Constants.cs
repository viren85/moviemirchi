using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Constants
{
    public class Constants
    {
        public static readonly string Field_Id = "Id";
        public static readonly string Field_EntityType = "EntityType";
        public static readonly string Field_EntityType_Movie = "Movies";
        public static readonly string Field_EntityType_Reviews = "Reviews";
        public static readonly string Field_EntityType_ReviewText = "ReviewsText";


        public static readonly string Field_Name = "MovieName";
        public static readonly string Field_AltNames = "AltMovieNames";
        public static readonly string Field_Actors = "MovieActors";
        public static readonly string Field_Directors = "MovieDirectors";
        public static readonly string Field_Producers = "MovieProducers";
        public static readonly string Field_MusicDirectors = "MusicDirectors";
        public static readonly string Field_MovieSynopsis = "MoviSynopsis";
        public static readonly string Field_ReviewerName = "ReviewerName";
        public static readonly string Field_Type = "Type";
        public static readonly string Field_Type_Movie = "Type_Movie";
        public static readonly string Field_Type_Review = "Type_Review";

        public static readonly List<string> MovieNameFilterList = new List<string> { { Field_Name }, { Field_AltNames } };
    }
}
