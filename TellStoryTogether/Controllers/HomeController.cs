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
        readonly UsersContext _userContext= new UsersContext();
        
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

/*        [HttpPost]
        public ActionResult LoadArticles(string genre)
        {
            try
            {
                var allArticles = _userContext.Articles.Include("Genre").Include("Owner").Where(p => p.Genre.Name == genre).Take(5).ToList();
                
                return
                    Json(allArticles);
            }
            catch (Exception)
            {

                return Json("error");
            }
        }*/


        [HttpPost]
        public ActionResult LoadGenre()
        {
            try
            {
                var genreWithMoreThan3 = _userContext.Articles.GroupBy(p => p.Genre).Select(group => new
                {
                    Genre = group.Key,
                    Count = group.Count()
                }).Where(p => p.Count > 3).Select(p => p.Genre.GenreId).ToList();

                return
                    Json(genreWithMoreThan3);
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
