using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index(string key,string value)
        {
            ViewBag.key = key;
            ViewBag.value = value;
            return View();
        }

        [HttpPost]
        public ActionResult LoadArticles(string key,string value, int from, int take)
        {
            DAL dal = new DAL();
            List<Article> articles = dal.GetArticles(key, value, true, from, take);
            return Json(articles);
        }

        [HttpPost]
        public ActionResult Remove(int articleId)
        {
            DAL dal = new DAL(User.Identity.Name);
            bool articleIsRemoved = dal.RemoveArticle(articleId);
            return Json(articleIsRemoved ? Message.Removed : Message.ServerRejected());
        }
    }
}
