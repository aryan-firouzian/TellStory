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
            DAL dal;
            List<HomeFeed> homeFeeds = new List<HomeFeed>();
            bool informedUser = false;
            int numUserArticle = 0;
            int userId = -1;
            if (User.Identity.IsAuthenticated)
            {
                dal = new DAL(User.Identity.Name);
                informedUser = dal.IsInformedUser();
                numUserArticle = dal.NumberOfArticles("Me", "Me");
                userId = dal.UserId();
            }
            else
            {
                dal = new DAL();
            }

            if (numUserArticle >= 3 && userId>0)
            {
                HomeFeed homeFeedMyStories = new HomeFeed
                {
                    PropKey = "User",
                    PropValue = userId.ToString(),
                    PropTitle = "My Stories",
                    PropDescription = "Best of my stories",
                    From = 0,
                    Take = 7
                };
                homeFeeds.Add(homeFeedMyStories);
            }
            else
            {
                if (!informedUser)
                {
                    HomeFeed homeFeedJumbo1 = new HomeFeed
                    {
                        PropKey = "Jumbo",
                        PropValue = "Get started!",
                        PropTitle = "How it works ...",
                        PropDescription =
                            "It is a crowdsourcing story telling service. People create stories together, which go to different directions.",
                        PropDescription2 = "Home/About",
                        From = 0,
                        Take = 0
                    };
                    homeFeeds.Add(homeFeedJumbo1);
                }
            }

            HomeFeed homeFeedBest = new HomeFeed
            {
                PropKey = "Best",
                PropValue = "Best",
                PropTitle = "Best Stories",
                PropDescription = "Best stories in all genres",
                From = 0,
                Take = 7
            };
            homeFeeds.Add(homeFeedBest);

            List<Genre> genreWithMoreThan3 = dal.GetGenreIdsWithMoreThan3();
            foreach (Genre genre in genreWithMoreThan3)
            {
                HomeFeed homeFeedGenre = new HomeFeed
                {
                    PropKey = "Genre",
                    PropValue = genre.GenreId.ToString(),
                    PropTitle = genre.Name,
                    PropDescription = genre.Detail,
                    From = 0,
                    Take = 5
                };
                homeFeeds.Add(homeFeedGenre);
            }

            return View(homeFeeds);
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
