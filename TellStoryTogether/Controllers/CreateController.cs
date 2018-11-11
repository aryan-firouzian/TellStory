using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Filters;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Controllers
{
    public class CreateController : Controller
    {
        //
        // GET: /Create/
        readonly UsersContext _userContext = new UsersContext();

        public ActionResult Index(string identifier)
        {
            List<Article> articles = new List<Article>();
            int length = 0;
            if (identifier == "new")
            {

            }
            else
            {
                List<int> parallels = identifier.Split('-').Select(Int32.Parse).ToList();
                int articleId = parallels[0];
                Article articleTemp = _userContext.Articles.Include("Owner").First(p => p.ArticleId == articleId);
                articles.Add(articleTemp);
                parallels.RemoveAt(0);
                length++;
                foreach (int parallel in parallels)
                {
                    articleTemp = _userContext.Articles.Include("Owner").First(p => p.ArticleInitId == articleId && p.Parallel == parallel);
                    articles.Add(articleTemp);
                    length++;
                    articleId = articleTemp.ArticleId;
                }
            }
            ViewBag.length = length;
            ViewBag.title = articles.Count == 0 ? "" : articles[0].Title;
            ViewBag.charMin = articles.Count == 0 ? "300" : articles[0].MinChar.ToString();
            ViewBag.charMax = articles.Count == 0 ? "2000" : articles[0].MaxChar.ToString();
            ViewBag.articleInitId = articles.Count == 0 ? -1 : articles.Last().ArticleId;
            ViewBag.serial = articles.Count == 0 ? 1 : articles.Last().Serial + 1;
            ViewBag.identifier = identifier;
            return View(articles);
        }


        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult SaveArticle(string identifier,HttpPostedFileBase blob, string title, int articleInitId, string text, int serial, int min, int max)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Guid guid = Guid.NewGuid();
                    string uniqueString = guid.ToString();
                    var fullPath = blob == null ? null : "/Images/StoryImage/" + uniqueString + ".png";
                    int parallel = 1;
                    if (articleInitId != -1)
                    {
                        parallel = _userContext.Articles.Count(p => p.ArticleInitId == articleInitId) + 1;
                    }
                    int userId = WebSecurity.GetUserId(User.Identity.Name);
                    UserProfile user = _userContext.UserProfiles.First(p => p.UserId == userId);
                    Genre genre = _userContext.Genres.First();
                    
                    Article newArticle = new Article
                    {
                        ArticleInitId = articleInitId,
                        Title = title,
                        Text = text,
                        PictureUrl = fullPath,
                        Point = 0,
                        Seen = 0,
                        Serial = serial,
                        Parallel = parallel,
                        Favorite = 0,
                        Selected = false,
                        Owner = user,
                        Genre = genre,
                        Time = DateTime.Now,
                        MinChar = min,
                        MaxChar = max
                    };
                    _userContext.Articles.Add(newArticle);
                    _userContext.SaveChanges();
                    if (blob != null) blob.SaveAs(Server.MapPath("~" + fullPath));
                    string newIdentifier = articleInitId == -1
                        ? newArticle.ArticleId.ToString()
                        : identifier + "-" + parallel;
                    _userContext.Articles.First(p => p.ArticleId == newArticle.ArticleId).Identifier = newIdentifier;
                    _userContext.SaveChanges();
                    return Json(new[]
                    {
                        "added",
                        newIdentifier
                    });
                }
                return Json(new[]
                {
                    "rejected",
                    "The request from an unauthenticated user. Log in or Register!"
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
    }
}
