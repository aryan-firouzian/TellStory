using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class EditController : Controller
    {
        //
        // GET: /Edit/

        public ActionResult Index(string aricleId)
        {
            DAL dal = new DAL(User.Identity.Name);
            Article article = dal.GetArticles("ArticleId", aricleId, true, 0, 1).First();
            ViewBag.genres = dal.GetGenres();
            ViewBag.languages = dal.GetLanguages();
            return dal.UserId() == article.Owner.UserId ? View(article) : View();
        }


        [HttpPost]
        public ActionResult EditArticle(int articleId, HttpPostedFileBase blob, string title, string text, int min, int max, int languageId, int genreId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);
                DAL dal = new DAL(User.Identity.Name);

                // save article in database
                dal.EditArticle(articleId, ref blob, title, text, min, max, languageId, genreId);
                // save image in the server
                if (blob != null) blob.SaveAs(Server.MapPath("~" + dal.CurrentArticle.PictureUrl));
                // get the new identifier
                string newIdentifier = dal.CurrentArticle.Identifier;
                return Json(Message.AddedWithMessage(newIdentifier));
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }

        }

    }


}
