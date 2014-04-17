using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;

namespace DataStoreLib.Storage
{
    internal class RandomMovieDataGenerator
    {
        public List<string> Actors = new List<string>
            {
                "SRK",
                "Sallu",
                "Amir Khan",
                "Kamal Hassan"
            };

        public List<string> Directors = new List<string>()
            {
                "Speilberg",
                "Gowarikar",
                "Vidhu Vinod Chopra"
            };

        public List<string> MusicDirecotrs = new List<string>()
            {
                "Shankar, Eshan Loy",
                "Rahman",
                "Copycat",
                "Bappi lahiri"
            };

        public List<string> Name = new List<string>()
            {
                "Sholay",
                "Mission",
                "Race",
                "Karz",
                "shaadi"
            };

        public List<string> Connectors = new List<string>()
            {
                "and",
                "of the",
                "or",
                "and the"
            };

        public List<string> Producers = new List<string>()
            {
                "Rich dude 1",
                "Karan Johar",
                "Mayawati"
            };

        public List<string> Reviewers = new List<string>
            {
                "reviewer 1",
                "fake 1",
                "biased 1"
            };

        public List<string> ReviewIds = new List<string>()
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };


        public MovieEntity GetRandomMovieEntity(string id)
        {
            var entity = new MovieEntity(id);
            var rand = new Random();

            entity.Posters = GetRandomElementsFromList(Actors, rand);
            entity.Songs = rand.Next(10).ToString();
            entity.Ratings = GetRandomElementsFromList(Directors, rand);
            entity.Trailers = "Trailers";
            entity.MovieId = id;
            entity.Casts = GetRandomElementsFromList(MusicDirecotrs, rand);
            entity.Name = GetRandomMovieName(Name, rand);
            entity.Synopsis = GetRandomElementsFromList(Producers, rand);

            entity.Stats = GetRandomElementsFromList(ReviewIds, rand);


            return entity;
        }

        private string GetRandomElementsFromList(List<string> items, Random r)
        {
            var iter = r.Next(items.Count);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iter; i++)
            {
                var itemIndex = r.Next(items.Count);
                if (sb.ToString().Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(items[itemIndex]);
            }
            return sb.ToString();
        }

        private string GetRandomMovieName(List<string> items, Random r)
        {
            var iter = r.Next(items.Count);
            var sb = new StringBuilder();

            for (int i = 0; i < iter; i++)
            {
                var itemIndex = r.Next(items.Count);
                if (sb.ToString().Length > 0)
                {
                    var connectorIndex = r.Next(Connectors.Count);
                    sb.Append(Connectors[connectorIndex]);
                }
                sb.Append(items[itemIndex]);
            }
            return sb.ToString();
        }

        public ReviewEntity GetRandomReview(string id)
        {
            var review = new ReviewEntity(id);
            var rand = new Random();

            review.ReviewId = id;
            review.Review = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas libero erat, elementum at dictum quis, commodo eget augue. Suspendisse potenti. Sed a tempor magna. Maecenas quis metus ac sapien faucibus eleifend. Mauris id elementum augue, ac suscipit nunc. Sed non lorem at turpis pellentesque tempus gravida vitae lectus. Nullam quis blandit augue. Proin at pretium magna, quis rutrum arcu. Praesent consectetur aliquam magna, fermentum sodales dolor.";
            review.ReviewerName = GetRandomElementsFromList(Reviewers, rand);
            review.ReviewId = Guid.NewGuid().ToString();
            review.ReviewerRating = rand.Next(10).ToString();
            review.SystemRating = rand.Next(10);

            return review;
        }
    }
}