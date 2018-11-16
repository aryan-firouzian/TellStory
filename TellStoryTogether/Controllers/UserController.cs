using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Filters;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;
using WebMatrix.WebData;

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

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult LoadFavoriteArticles(int take)
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            var queryFavoriteIds =
                _userContext.ArticleFavorites.Where(p => p.User.UserId == userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            int count = queryFavoriteIds.Count();
            List<int> favoriteIds = queryFavoriteIds.TakeLast(take).ToList();
            favoriteIds.Reverse();
            List<Article> articles =
                favoriteIds.Select(p => _userContext.Articles.First(q => q.ArticleId == p)).ToList();
            return Json(new
            {
                count,
                articles
            });
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult LoadScriptArticles(int take)
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            var queryArticles = _userContext.Articles.Where(p => p.Owner.UserId == userId);
            int count = queryArticles.Count();
            List<Article> articles = queryArticles.TakeLast(take).ToList();
            articles.Reverse();
            return Json(new
            {
                count,
                articles
            });
        }
    }
}
