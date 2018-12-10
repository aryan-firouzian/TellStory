using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class GenreController : Controller
    {
        //
        // GET: /Genre/
        public ActionResult Index(int genreId)
        {
            ViewBag.GenreId = genreId;
            return View();
        }

        [HttpPost]
        public ActionResult LoadArticles(int genreId, int from, int take)
        {
            DAL dal = new DAL();
            List<Article> articles = dal.GetArticles("Genre", genreId, true, from, take);
            return Json(articles);
        }

    }
}
