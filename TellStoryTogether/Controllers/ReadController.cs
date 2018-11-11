using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Controllers
{
    public class ReadController : Controller
    {
        //
        // GET: /Read/
        readonly UsersContext _userContext = new UsersContext();

        public ActionResult Index(string identifier)
        {
            string[] articleIdIdentifier = Identifier(identifier);
            int firstArticleId = Int32.Parse(articleIdIdentifier[0]);
            string restArticle = articleIdIdentifier[1];
            
            Article firstArticle = _userContext.Articles.Include("Owner").First(p => p.ArticleId == firstArticleId);
            ViewBag.Identifier = restArticle;
            return View(firstArticle);
        }

        public ActionResult ArticleAttach(string identifierOrArticleId)
        {
            string identifier = Identifier(identifierOrArticleId)[1];
            List<List<Article>> output = new List<List<Article>>();
            List<int> parallelList = identifier.Split('-').Select(Int32.Parse).ToList();
            int sourceId = parallelList[0];
            parallelList.RemoveAt(0);
            foreach (int i in parallelList)
            {
                List<Article> tempArticles = _userContext.Articles.Include("Owner").Where(p => p.ArticleInitId == sourceId).OrderByDescending(p => p.Parallel == i).ThenByDescending(p => p.Point).ToList();
                sourceId = tempArticles[0].ArticleId;
                output.Add(tempArticles);
            }
            return Json(output);
        }

        public ActionResult LoadComment(string articleId)
        {
            int articleIdInt = Int32.Parse(articleId);
            List<Comment> comments = _userContext.Comments.Include("User").Where(p => p.ArticleId.ArticleId == articleIdInt).ToList();
            return Json(comments);
        }

        [HttpPost]
        public ActionResult SaveComment(int articleId, string content)
        {
            /*try
            {*/
                if (User.Identity.IsAuthenticated)
                {
                    int userId = WebSecurity.GetUserId(User.Identity.Name);
                    Comment comment = new Comment
                    {
                        ArticleId = _userContext.Articles.First(p => p.ArticleId == articleId),
                        Content = content,
                        User = _userContext.UserProfiles.First(p => p.UserId == userId)
                    };


                    _userContext.Comments.Add(comment);
                    _userContext.SaveChanges();
                    return Json(new[]
                    {
                        "added",
                        "no message"
                    });
                }
                return Json(new[]
                {
                    "rejected",
                    "The request from an unauthenticated user. Log in or Register!"
                });
            /*}
            catch (Exception e)
            {
                return Json(new[]
                    {
                        "rejected",
                        e.Message
                    });
            }*/
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
