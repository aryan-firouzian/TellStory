using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TellStoryTogether.Helper;

namespace TellStoryTogether.Controllers
{
    public class ReadController : Controller
    {
        //
        // GET: /Read/
        public ActionResult Index(string identifier)
        {
            DAL dal = new DAL(User.Identity.Name);
            string[] articleIdIdentifier = dal.SplitFirstId_PopulateNewIdentifier(identifier);
            
            int firstArticleId = Int32.Parse(articleIdIdentifier[0]);
            string newIdentifier = articleIdIdentifier[1];

            ArticleUserBase firstArticle = dal.GetArticleUserBaseById(firstArticleId);

            ViewBag.TextDirection = firstArticle.Language.RightToLeft ? "rtl" : "ltr";
            ViewBag.Identifier = newIdentifier;
            return View(firstArticle);
        }

        public ActionResult ArticleAttach(string identifierOrArticleId)
        {
            DAL dal = new DAL(User.Identity.Name);
            string identifier = dal.SplitFirstId_PopulateNewIdentifier(identifierOrArticleId)[1];
            List<List<ArticleUserBase>> restArticles = dal.GetTailArticleUserBaseByIdentifier(identifier);

            return Json(restArticles);
        }

        public ActionResult LoadComment(string articleId)
        {
            DAL dal = new DAL();
            List<CommentTime> comments = dal.GetCommentsByArticleId(articleId);
            return Json(comments);
        }


        [HttpPost]
        public ActionResult SaveComment(int articleId, string content)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);
                
                DAL dal = new DAL(User.Identity.Name);
                dal.SaveComment(articleId, content);
                
                dal.SubscribeNotification("Comment");
                dal.AddNotificationRecord("Comment");
                return Json(Message.Added);
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
        }

        [HttpPost]
        public ActionResult Like(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);

                DAL dal = new DAL(User.Identity.Name);
                if (dal.Like(articleId))
                {
                    dal.SubscribeNotification("Like");
                    return Json(Message.Added);
                }
                return Json(Message.ServerRejected());
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
        }

        [HttpPost]
        public ActionResult UnLike(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);

                DAL dal = new DAL(User.Identity.Name);
                if(dal.UnLike(articleId))
                {
                    dal.SubscribeNotification("UnLike");
                    return Json(Message.Removed);
                }
                return Json(Message.ServerRejected());
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
        }

        [HttpPost]
        public ActionResult Star(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);

                DAL dal = new DAL(User.Identity.Name);
                if(dal.Star(articleId))
                {
                    dal.SubscribeNotification("Favorite");
                    dal.AddNotificationRecord("Favorite");
                    return Json(Message.Added);
                }
                return Json(Message.ServerRejected());
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
        }

        [HttpPost]
        public ActionResult UnStar(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(Message.UnAuthenticated);

                DAL dal = new DAL(User.Identity.Name);
                if (dal.UnStar(articleId))
                {
                    dal.SubscribeNotification("UnFavorite");
                    dal.RemoveNotificationRecord("Favorite");
                    return Json(Message.Removed);
                }
                return Json(Message.ServerRejected());
            }
            catch (Exception e)
            {
                return Json(Message.ServerRejected(e));
            }
        }



    }
}
