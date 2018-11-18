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
    public class FavoriteController : Controller
    {
        readonly UsersContext _userContext = new UsersContext();
        //
        // GET: /Favorite/
        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            var queryFavoriteIds =
                _userContext.ArticleFavorites.Where(p => p.User.UserId == userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            List<int> favoriteIds = queryFavoriteIds.ToList();
            favoriteIds.Reverse();
            List<Article> articles =
                favoriteIds.Select(p => _userContext.Articles.First(q => q.ArticleId == p)).ToList();
            return View(articles);
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
    }
}
