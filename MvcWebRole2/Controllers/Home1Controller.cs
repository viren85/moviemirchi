using DataStoreLib.Storage;
using MvcWebRole2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole2.Controllers
{
    public class Home1Controller : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        #region movie input section
        
        public ActionResult Movie_ReviewInput()
        {
            return View(new MovieModel { MovieId = "", MovieName = "Enter name here", AltMovieNames = "" });
        }

        [HttpGet]
        public ActionResult Submit_MovieInput(MovieModel model)
        {
            if (!model.Validate())
            {
                @ViewBag.ValidationError = "Check details and submit again";
                return View(model);
            }

            return AddMovie(model);
        }

        public ActionResult AddMovie(MovieModel model)
        {
            if (string.IsNullOrEmpty(model.MovieId))
            {
                model.MovieId = Guid.NewGuid().ToString();
            }
            return AddReview(model);
        }

        public ActionResult AddReview(MovieModel moviemodel)
        {
            var model = new ReviewModel();
            model.MovieId = moviemodel.MovieId;
            model.ReviewId = Guid.NewGuid().ToString();

            return View("AddReview", model);
        }

        [HttpPost]
        public ActionResult Submit_Review(ReviewModel reviewModel)
        {
            return View(reviewModel);
        }

        #endregion 


        #region personality input section

        public ActionResult Personality()
        {
            return View();
        }

        public ActionResult Personality_ReviewInput()
        {
            return View();
        }

        #endregion
    }
}
