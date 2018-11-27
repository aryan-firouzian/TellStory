using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TellStoryTogether.Models;
using WebGrease.Css.Extensions;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    public class DAL
    {
        readonly UsersContext _context = new UsersContext();

        private readonly UserProfile _user;
        private readonly int _userId = -1;
        public Article CurrentArticle;

        public DAL()
        {
            
        }

        public DAL(string name)
        {
            if (!WebSecurity.IsAuthenticated) return;
            _userId = WebSecurity.GetUserId(name);
            _user = _context.UserProfiles.First(p => p.UserId == _userId);
        }

        /*public UserProfile GetUser(string name)
        {
             return _user;
        }*/

        public List<Article> GetArticlesByIdentifier(string identifier)
        {
            List<Article> articles = new List<Article>();

            if (identifier == "new")
            {

            }
            else
            {
                List<int> parallels = identifier.Split('-').Select(Int32.Parse).ToList();
                int articleId = parallels[0];
                Article articleTemp = _context.Articles.Include("Owner").Include("Genre").First(p => p.ArticleId == articleId);
                articles.Add(articleTemp);
                parallels.RemoveAt(0);
                foreach (int parallel in parallels)
                {
                    articleTemp = _context.Articles.Include("Owner").Include("Genre").First(p => p.ArticleInitId == articleId && p.Parallel == parallel);
                    articles.Add(articleTemp);
                    articleId = articleTemp.ArticleId;
                }
            }
            return articles;
        }

        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public ArticleUserBase GetArticleUserBaseById(int articleId)
        {
            List<int> userComments = _context.Comments.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userPoints = _context.ArticlePoints.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userFavorites = _context.ArticleFavorites.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();

            return _context.Articles.Include("Owner").First(p => p.ArticleId == articleId).ArticleToArticleUser(userPoints, userFavorites, userComments);
        }

        public void SaveArticle(ref HttpPostedFileBase blob, string title, int articleInitId, string text, int serial, int min, int max, int genreId, string identifier)
        {
            Guid guid = Guid.NewGuid();
            string uniqueString = guid.ToString();
            var fullPath = blob == null ? null : "/Images/StoryImage/" + uniqueString + ".png";
            int parallel = 1;
            if (articleInitId != -1)
            {
                parallel = _context.Articles.Count(p => p.ArticleInitId == articleInitId) + 1;
            }

            Genre genre = _context.Genres.First(p => p.GenreId == genreId);

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
                Owner = _user,
                Genre = genre,
                Time = DateTime.Now,
                MinChar = min,
                MaxChar = max
            };
            _context.Articles.Add(newArticle);
            _context.SaveChanges();
            string newIdentifier = newArticle.ArticleInitId == -1
                ? newArticle.ArticleId.ToString()
                : identifier + "-" + newArticle.Parallel;
            newArticle.Identifier = newIdentifier;
            _context.SaveChanges();
            CurrentArticle = newArticle;
        }

        public string[] SplitFirstId_PopulateNewIdentifier(string articleIdOrIdentifier)
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
                    tempId = _context.Articles.First(p => p.ArticleInitId == tempId && p.Parallel == parallel).ArticleId;
                }
                identifier = articleIdOrIdentifier;
                var tempArticles = _context.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _context.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
            }
            else
            {
                articleId = articleIdOrIdentifier;
                int tempId = Int32.Parse(articleId);
                var tempArticles = _context.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                while (tempArticles.Count != 0)
                {
                    Article tempArticle = tempArticles[0];
                    tempId = tempArticle.ArticleId;
                    identifier = identifier + "-" + tempArticle.Parallel;
                    tempArticles = _context.Articles.Where(p => p.ArticleInitId == tempId).OrderByDescending(p => p.Point).ToList();
                }
                identifier = articleId + identifier;
            }
            return new[] { articleId, identifier };
        }

        public void SubscribeForkNotificationForEarlierArticles(string identifier)
        {
            if (identifier == "new") return;
            List<int> s = identifier.Split('-').Select(Int32.Parse).ToList();
            int articleId = s[0];
            _context.Notifications.Where(
                p =>
                    p.Article.ArticleId == articleId && p.Seen == false &&
                    (p.State == "All" || p.State == "Favorite")).ForEach(p => p.Forked++);
            s.RemoveAt(0);
            int articleIntId = articleId;
            foreach (int parallel in s)
            {
                var parallel1 = parallel;
                var nextArticle =
                    _context.Articles.First(p => p.ArticleInitId == articleIntId && p.Parallel == parallel1);
                _context.Notifications.Where(p => p.Article.ArticleId == nextArticle.ArticleId && p.Seen == false)
                    .ForEach(p => p.Forked++);
                articleIntId = nextArticle.ArticleId;
            }
            _context.SaveChanges();
        }

        public void AddNotificationRecord(string state)
        {
            int userId = _user.UserId;
            int articleId = CurrentArticle.ArticleId;
            if (_context.Notifications.Any(p => p.User.UserId == userId && p.Article.ArticleId == articleId && p.State == "All"))
            {

            }
            else
            {
                Notification notification = new Notification
                {
                    User = _user,
                    Article = CurrentArticle,
                    Seen = false,
                    State = state,
                    Time = DateTime.Now,
                    Identifier = CurrentArticle.Identifier
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();
            }

        }

        public void RemoveNotificationRecord(string state)
        {
            int articleId = CurrentArticle.ArticleId;
            IQueryable<Notification> notification =
        _context.Notifications.Where(
            p => p.User.UserId == _userId && p.Article.ArticleId == articleId && p.State == state);
            _context.Notifications.RemoveRange(notification);
            _context.SaveChanges();
        }

        public List<List<ArticleUserBase>> GetTailArticleUserBaseByIdentifier(string identifier)
        {
            List<int> userComments = _context.Comments.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userPoints = _context.ArticlePoints.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userFavorites = _context.ArticleFavorites.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();

            List<List<ArticleUserBase>> output = new List<List<ArticleUserBase>>();
            List<int> parallelList = identifier.Split('-').Select(Int32.Parse).ToList();
            int sourceId = parallelList[0];
            parallelList.RemoveAt(0);
            foreach (int i in parallelList)
            {
                List<ArticleUserBase> tempArticles = _context.Articles.Include("Owner").Where(p => p.ArticleInitId == sourceId).ArticlesToArticleUsers(userPoints, userFavorites, userComments).OrderByDescending(p => p.Parallel == i).ThenByDescending(p => p.Point).ToList();
                sourceId = tempArticles[0].ArticleId;
                output.Add(tempArticles);
            }
            return output;
        }

        public List<CommentTime> GetCommentsByArticleId(string articleId)
        {
            int articleIdInt = Int32.Parse(articleId);
            return _context.Comments.Include("User").Where(p => p.Article.ArticleId == articleIdInt).ToList().ChangeTime().ToList();
        }

        public void SaveComment(int articleId, string content)
        {
            Article article = _context.Articles.First(p => p.ArticleId == articleId);
            CurrentArticle = article;
            Comment comment = new Comment
            {
                Article = article,
                Content = content,
                User = _user,
                Time = DateTime.Now
            };
            _context.Comments.Add(comment);
            _context.Articles.First(p => p.ArticleId == articleId).Comment++;
            _context.SaveChanges();
        }

        public void SubscribeNotification(string action)
        {
            int articleId = CurrentArticle.ArticleId;
            switch (action)
            {
                case "Comment":
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false).ForEach(p => p.Commented++);
                    _context.SaveChanges();
                    break;
                case "Like":
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Liked++);
                    _context.SaveChanges();
                    break;
                case "UnLike":
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Liked--);
                    _context.SaveChanges();
                    break;
                case "Favorite":
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Favorited++);
                    _context.SaveChanges();
                    break;
                case "UnFavorite":
                    _context.Notifications.Where(p=>p.Article.ArticleId==articleId&&p.Seen==false&&p.State=="All").ForEach(p=>p.Favorited--);
                    _context.SaveChanges();
                    break;
            }
        }

        public void Like(int articleId)
        {
            CurrentArticle = _context.Articles.First(p => p.ArticleId == articleId);
            ArticlePoint articlePoint = new ArticlePoint()
            {
                Article = CurrentArticle,
                User = _user
            };
            _context.ArticlePoints.Add(articlePoint);
            CurrentArticle.Point++;
            _context.SaveChanges();
        }

        public void UnLike(int articleId)
        {
            CurrentArticle = _context.Articles.First(p => p.ArticleId == articleId);
            ArticlePoint articlePoint =
    _context.ArticlePoints.First(p => p.Article.ArticleId == articleId && p.User.UserId == _userId);
            _context.ArticlePoints.Remove(articlePoint);
            CurrentArticle.Point--;
            _context.SaveChanges();
        }

        public void Star(int articleId)
        {

            CurrentArticle = _context.Articles.First(p => p.ArticleId == articleId);
            ArticleFavorite articleFavorite = new ArticleFavorite()
            {
                Article = CurrentArticle,
                User = _user
            };
            _context.ArticleFavorites.Add(articleFavorite);
            CurrentArticle.Favorite++;
            _context.SaveChanges();
        }

        public void UnStar(int articleId)
        {
            CurrentArticle = _context.Articles.First(p => p.ArticleId == articleId);
            ArticleFavorite articleFavorite = _context.ArticleFavorites.First(p => p.Article.ArticleId == articleId && p.User.UserId == _userId);
            _context.ArticleFavorites.Remove(articleFavorite);
            CurrentArticle.Favorite--;
            _context.SaveChanges();
        }


        public List<Article> GetFavoriteArticle()
        {
            var queryFavoriteIds =
                _context.ArticleFavorites.Where(p => p.User.UserId == _userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            List<int> favoriteIds = queryFavoriteIds.ToList();
            favoriteIds.Reverse();
            return favoriteIds.Select(p => _context.Articles.First(q => q.ArticleId == p)).ToList();
        }

        public Tuple<List<Article>, int> GetFirstNFavoriteArticle(int take)
        {
            var queryFavoriteIds =
                _context.ArticleFavorites.Where(p => p.User.UserId == _userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            int count = queryFavoriteIds.Count();
            List<int> favoriteIds = queryFavoriteIds.TakeLast(take).ToList();
            favoriteIds.Reverse();
            return new Tuple<List<Article>, int>(favoriteIds.Select(p => _context.Articles.First(q => q.ArticleId == p)).ToList(),count);
        }

        public List<Article> GetScriptArticle()
        {
            var queryArticles = _context.Articles.Where(p => p.Owner.UserId == _userId);
            List<Article> articles = queryArticles.ToList();
            articles.Reverse();
            return articles;
        }

        public Tuple<List<Article>, int> GetFirstNScriptArticle(int take)
        {
            var queryArticles = _context.Articles.Where(p => p.Owner.UserId == _userId);
            int count = queryArticles.Count();
            List<Article> articles = queryArticles.TakeLast(take).ToList();
            articles.Reverse();
            return new Tuple<List<Article>, int>(articles, count);
        }
    }
}