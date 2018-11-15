using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        readonly UsersContext _userContext = new UsersContext();

        public ActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public ActionResult LoadArticles(int userId, int from, int take)
        {
            List<Article> articles = _userContext.Articles.Where(p => p.ArticleInitId == -1 && p.Owner.UserId == userId).OrderBy(p => p.Point).Skip(from).Take(take).Include(p => p.Owner).Include(p => p.Genre).ToList();
            return Json(articles);
        }

    }
}
