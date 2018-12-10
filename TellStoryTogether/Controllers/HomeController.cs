using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class HomeController : Controller
    {   
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult LoadGenreIdsWithMoreThan3()
        {
            try
            {
                DAL dal = new DAL();
                var genreWithMoreThan3 = dal.GetGenreIdsWithMoreThan3();
                return Json(genreWithMoreThan3);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SeeNotifications()
        {
            try
            {
                DAL dal = new DAL(User.Identity.Name);
                bool done = dal.SeeNotification();
                return Json(done ? "done" : "error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }
}
