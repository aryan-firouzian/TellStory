using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Filters;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Controllers
{
    public class ScriptController : Controller
    {
        //
        // GET: /Script/
        readonly UsersContext _userContext = new UsersContext();

        public ActionResult Index()
        {
            int userId = WebSecurity.GetUserId(User.Identity.Name);
            var queryArticles = _userContext.Articles.Where(p => p.Owner.UserId == userId);
            List<Article> articles = queryArticles.ToList();
            articles.Reverse();
            return View(articles);
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
