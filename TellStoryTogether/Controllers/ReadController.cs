using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TellStoryTogether.Filters;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;
using System.Data.Entity;
using WebGrease.Css.Extensions;
using WebMatrix.WebData;

namespace TellStoryTogether.Controllers
{
    public class ReadController : Controller
    {
        //
        // GET: /Read/
        readonly UsersContext _userContext = new UsersContext();

        [InitializeSimpleMembership]
        public ActionResult Index(string identifier)
        {
            string[] articleIdIdentifier = Identifier(identifier);
            int firstArticleId = Int32.Parse(articleIdIdentifier[0]);
            string restArticle = articleIdIdentifier[1];

            int userId = -1;
            if (User.Identity.IsAuthenticated)
            {
                userId = WebSecurity.GetUserId(User.Identity.Name);
            }
            List<int> userComments = _userContext.Comments.Where(p => p.User.UserId == userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userPoints = _userContext.ArticlePoints.Where(p => p.User.UserId == userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userFavorites = _userContext.ArticleFavorites.Where(p => p.User.UserId == userId).Select(p => p.Article.ArticleId).ToList();

            ArticleUserBase firstArticle = _userContext.Articles.Include("Owner").First(p => p.ArticleId == firstArticleId).ArticleToArticleUser(userPoints,userFavorites,userComments);
            ViewBag.Identifier = restArticle;
            return View(firstArticle);
        }

        [InitializeSimpleMembership]
        public ActionResult ArticleAttach(string identifierOrArticleId)
        {
            int userId = -1;
            if (User.Identity.IsAuthenticated)
            {
                userId = WebSecurity.GetUserId(User.Identity.Name);
            }
            List<int> userComments = _userContext.Comments.Where(p => p.User.UserId == userId).Select(p=>p.Article.ArticleId).ToList();
            List<int> userPoints = _userContext.ArticlePoints.Where(p => p.User.UserId == userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userFavorites = _userContext.ArticleFavorites.Where(p => p.User.UserId == userId).Select(p => p.Article.ArticleId).ToList();

            string identifier = Identifier(identifierOrArticleId)[1];
            List<List<ArticleUserBase>> output = new List<List<ArticleUserBase>>();
            List<int> parallelList = identifier.Split('-').Select(Int32.Parse).ToList();
            int sourceId = parallelList[0];
            parallelList.RemoveAt(0);
            foreach (int i in parallelList)
            {


                List<ArticleUserBase> tempArticles = _userContext.Articles.Include("Owner").Where(p => p.ArticleInitId == sourceId).ArticlesToArticleUsers(userPoints,userFavorites,userComments).OrderByDescending(p => p.Parallel == i).ThenByDescending(p => p.Point).ToList();
                sourceId = tempArticles[0].ArticleId;
                output.Add(tempArticles);
            }
            return Json(output);
        }

        public ActionResult LoadComment(string articleId)
        {
            int articleIdInt = Int32.Parse(articleId);
            var comments = _userContext.Comments.Include("User").Where(p => p.Article.ArticleId == articleIdInt).ToList().ChangeTime();
            return Json(comments);
        }


        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult SaveComment(int articleId, string content)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new[]
                    {
                        "rejected",
                        "The request from an unauthenticated user. Log in or Register!"
                    });
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                UserProfile user = _userContext.UserProfiles.First(p => p.UserId == userId);
                Article article = _userContext.Articles.First(p => p.ArticleId == articleId);
                Comment comment = new Comment
                {
                    Article = article,
                    Content = content,
                    User = user,
                    Time = DateTime.Now
                };
                _userContext.Comments.Add(comment);
                _userContext.Articles.First(p => p.ArticleId == articleId).Comment++;
                _userContext.SaveChanges();
                DbHelperNoContext.AddNotificationRecord(_userContext, user, article, "Comment");
                return Json(new[]
                {
                    "added",
                    "no message"
                });
            }
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult Like(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new[]
                    {
                        "rejected",
                        "The request from an unauthenticated user. Log in or Register!"
                    });
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                ArticlePoint articlePoint = new ArticlePoint()
                {
                    Article = _userContext.Articles.First(p => p.ArticleId == articleId),
                    User = _userContext.UserProfiles.First(p => p.UserId == userId)
                };
                _userContext.ArticlePoints.Add(articlePoint);
                _userContext.Articles.First(p => p.ArticleId == articleId).Point++;
                _userContext.SaveChanges();
                return Json(new[]
                {
                    "added",
                    "no message"
                });
            }
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult UnLike(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new[]
                    {
                        "rejected",
                        "The request from an unauthenticated user. Log in or Register!"
                    });
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                ArticlePoint articlePoint =
                    _userContext.ArticlePoints.First(p => p.Article.ArticleId == articleId && p.User.UserId == userId);
                _userContext.ArticlePoints.Remove(articlePoint);
                _userContext.Articles.First(p => p.ArticleId == articleId).Point--;
                _userContext.SaveChanges();
                return Json(new[]
                {
                    "removed",
                    "no message"
                });
            }
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult Star(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new[]
                    {
                        "rejected",
                        "The request from an unauthenticated user. Log in or Register!"
                    });
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                Article article = _userContext.Articles.First(p => p.ArticleId == articleId);
                UserProfile user = _userContext.UserProfiles.First(p => p.UserId == userId);
                ArticleFavorite articleFavorite = new ArticleFavorite()
                {
                    Article = article,
                    User = user
                };
                _userContext.ArticleFavorites.Add(articleFavorite);
                _userContext.Articles.First(p => p.ArticleId == articleId).Favorite++;
                _userContext.SaveChanges();
                DbHelperNoContext.AddNotificationRecord(_userContext, user, article, "Favorite");
                return Json(new[]
                {
                    "added",
                    "no message"
                });
            }
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }
        }

        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult UnStar(int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new[]
                    {
                        "rejected",
                        "The request from an unauthenticated user. Log in or Register!"
                    });
                int userId = WebSecurity.GetUserId(User.Identity.Name);
                ArticleFavorite articleFavorite = _userContext.ArticleFavorites.First(p => p.Article.ArticleId == articleId && p.User.UserId == userId);
                _userContext.ArticleFavorites.Remove(articleFavorite);
                _userContext.Articles.First(p => p.ArticleId == articleId).Favorite--;
                _userContext.SaveChanges();
                DbHelperNoContext.RemoveNotificationRecord(_userContext, userId, articleId, "Favorite");
                
                return Json(new[]
                {
                    "removed",
                    "no message"
                });
            }
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }
        }

        private string[] Identifier(string articleIdOrIdentifier)
        {
            string articleId;
            string identifier = "";
            if (articleIdOrIdentifier.Contains("-"))
            {
                List<int> tempList = articleIdOrIdentifier.Split('-').Select(Int32.Parse).ToList();
                int tempId = tempList[0];
                articleId = tempId.ToString();
                tempList.RemoveAt(0);
                foreach (int parallel in tempList)
                {
                    tempId = _userContext.Articles.First(p => p.ArticleInitId == tempId && p.Parallel==parallel).ArticleId;
                }
                identifier = articleIdOrIdentifier;
                var tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
            }
            else
            {
                articleId = articleIdOrIdentifier;
                int tempId = Int32.Parse(articleId);
                var tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _userContext.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
                identifier = articleId + identifier;
            }
            return new[] { articleId, identifier };
        }

    }
}
