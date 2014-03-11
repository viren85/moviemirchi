using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcWebRole2.Models
{
    public class MovieModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "MovieId")]
        public string MovieId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="MovieName")]
        public string MovieName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="AltMovieNames")]
        public string AltMovieNames { get; set; }

        public bool Validate()
        {
            bool isValidated = false;

            if (string.IsNullOrEmpty(MovieName))
            {
                isValidated = false;
            }
            isValidated = true;
            return isValidated;
        }
    }

    public class PersonalityModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Id;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Name")]
        public string Name;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Alternate names")]
        public string AltNames;

        [DataType(DataType.Text)]
        [Display(Name = "Names used in movie")]
        public IDictionary<string, string> SuggestedNames;
    }

    public class ReviewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "MovieId")]
        public string MovieId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ReviewId")]
        public string ReviewId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Reviewer")]
        public string ReviewerName{get;set;}

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name="ReviewText")]
        public string Review{get;set;}

        [Required]
        [Display(Name="Rating")]
        public int Rating{get;set;}
    }
}