using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class CreateController : Controller
    {
        //
        // GET: /Create/
        

        public ActionResult Index(string identifier)
        {
            DAL dal = new DAL(User.Identity.Name);
            List<Article> articles = dal.GetArticlesByIdentifier(identifier);

            ViewBag.length = articles.Count;
            ViewBag.title = articles.Count == 0 ? "" : articles[0].Title;
            ViewBag.charMin = articles.Count == 0 ? "300" : articles[0].MinChar.ToString();
            ViewBag.charMax = articles.Count == 0 ? "2000" : articles[0].MaxChar.ToString();
            ViewBag.articleInitId = articles.Count == 0 ? -1 : articles.Last().ArticleId;
            ViewBag.serial = articles.Count == 0 ? 1 : articles.Last().Serial + 1;
            ViewBag.genre = articles.Count == 0 ? -1 : articles[0].Genre.GenreId;
            ViewBag.identifier = identifier;

            ViewBag.genres = dal.GetGenres();
            return View(articles);
        }


        [HttpPost]
        public ActionResult SaveArticle(string identifier,HttpPostedFileBase blob, string title, int articleInitId, string text, int serial, int min, int max, int genreId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);
                DAL dal = new DAL(User.Identity.Name);

                // save article in database
                dal.SaveArticle(ref blob, title, articleInitId, text, serial, min, max, genreId, identifier);
                // save image in the server
                if (blob != null) blob.SaveAs(Server.MapPath("~" + dal.CurrentArticle.PictureUrl));
                // get the new identifier
                string newIdentifier = dal.CurrentArticle.Identifier;

                // informs previous articles in seri that there is a new article in tail
                dal.SubscribeForkNotificationForEarlierArticles(identifier);
                // store this article to get future notifications
                dal.AddNotificationRecord("All");

                return Json(Message.AddedWithMessage(newIdentifier));
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
            
        }
    }
}
