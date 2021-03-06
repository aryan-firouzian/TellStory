﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
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

        public DAL(int currentUserId)
        {
            if (!WebSecurity.IsAuthenticated) return;
            _userId = currentUserId;
            _user = _context.UserProfiles.First(p => p.UserId == _userId);
        }

        public int UserId()
        {
            return _userId;
        }

        public int UserPoint()
        {
            try
            {
                return _userId < 0 ? 0 : _context.UserProfiles.First(p => p.UserId == _userId).UserPoint;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        public List<Article> GetArticlesByIdentifier(string identifier)
        {
            List<Article> articles = new List<Article>();

            if (identifier == Constant.New)
            {

            }
            else
            {
                List<int> parallels = identifier.Split('-').Select(Int32.Parse).ToList();
                int articleId = parallels[0];
                Article articleTemp = _context.Articles.Include("Owner").Include("Genre").Include("Language").First(p => p.ArticleId == articleId);
                articles.Add(articleTemp);
                parallels.RemoveAt(0);
                foreach (int parallel in parallels)
                {
                    articleTemp = _context.Articles.Include("Owner").First(p => p.ArticleInitId == articleId && p.Parallel == parallel);
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

        public void SaveArticle(ref HttpPostedFileBase blob, string title, int articleInitId, string text, int serial, int min, int max, int languageId, int genreId, string identifier)
        {
            Guid guid = Guid.NewGuid();
            string uniqueString = guid.ToString();
            var fullPath = blob == null ? null : "/Images/StoryImage/" + uniqueString + ".png";
            int parallel = 1;
            if (articleInitId != -1)
            {
                parallel = _context.Articles.Count(p => p.ArticleInitId == articleInitId) + 1;
                _context.Articles.First(p => p.ArticleId == articleInitId).IsLast = false;
            }

            Language language = _context.Languages.First(p => p.LanguageId == languageId);
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
                Language = language,
                Genre = genre,
                Time = DateTime.Now,
                MinChar = min,
                MaxChar = max,
                IsLast = true
            };
            _context.Articles.Add(newArticle);
            _user.UserPoint = _user.UserPoint + Constant.CreatePoint;
            _context.SaveChanges();
            string newIdentifier = newArticle.ArticleInitId == -1
                ? newArticle.ArticleId.ToString()
                : identifier + "-" + newArticle.Parallel;
            newArticle.Identifier = newIdentifier;
            newArticle.TopArticleInitId = Int32.Parse(newArticle.Identifier.Split('-')[0]);
            _context.SaveChanges();
            CurrentArticle = newArticle;
        }

        public void EditArticle(int articleId, ref HttpPostedFileBase blob, string title, string text, int min, int max, int languageId, int genreId)
        {
            Article article = _context.Articles.First(p => p.ArticleId == articleId);
            Guid guid = Guid.NewGuid();
            string uniqueString = guid.ToString();
            var fullPath = blob == null ? article.PictureUrl : "/Images/StoryImage/" + uniqueString + ".png";
            
            Language language = _context.Languages.First(p => p.LanguageId == languageId);
            Genre genre = _context.Genres.First(p => p.GenreId == genreId);

            article.Title = title;
            article.Text = text;
            article.PictureUrl = fullPath;
            article.Language = language;
            article.Genre = genre;
            article.MinChar = min;
            article.MaxChar = max;

            if (article.ArticleInitId == -1)
            {
                var childArticles = _context.Articles.Where(p => p.TopArticleInitId == articleId);
                foreach (Article childArticle in childArticles)
                {
                    childArticle.Title = title;
                    childArticle.Language = language;
                    childArticle.Genre = genre;
                    childArticle.MinChar = min;
                    childArticle.MaxChar = max;
                }
            }
            _context.SaveChanges();
            CurrentArticle = article;
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

        public void AddNotificationRecord(string state)
        {
            int articleId = CurrentArticle.ArticleId;
            switch (state)
            {
                case Constant.CreateState:
                    Notification newCreateNotif = new Notification
                    {
                        User = _user,
                        Article = CurrentArticle,
                        Visited = false,
                        Seen = false,
                        CreateState = true,
                        Time = DateTime.Now,
                        ForkedArticleIds = "",
                        Content = "",
                        Identifier = CurrentArticle.Identifier
                    };
                    _context.Notifications.Add(newCreateNotif);
                    break;

                case Constant.CommentState:
                    if (_context.Notifications.Any(p =>
                        p.User.UserId == _userId && p.Article.ArticleId == articleId &&
                        (p.CreateState || p.CommentState)))
                    {
                        return;
                    }
                    var favoriteNotifs =
                        _context.Notifications.Where(
                            p => p.User.UserId == _userId && p.Article.ArticleId == articleId && p.FavoriteState);
                    if (favoriteNotifs.Any())
                    {
                        foreach (Notification favoriteNotif in favoriteNotifs)
                        {
                            favoriteNotif.CommentState = true;
                        }
                    }
                    else
                    {
                        Notification newCommentNotif = new Notification
                        {
                            User = _user,
                            Article = CurrentArticle,
                            Visited = false,
                            Seen = false,
                            CommentState = true,
                            Time = DateTime.Now,
                            ForkedArticleIds = "",
                            Content = "",
                            Identifier = CurrentArticle.Identifier
                        };
                        _context.Notifications.Add(newCommentNotif);
                    }
                    break;

                case Constant.FavoriteState:
                    if (_context.Notifications.Any(p =>
                        p.User.UserId == _userId && p.Article.ArticleId == articleId &&
                        (p.CreateState||p.FavoriteState)))
                    {
                        return;
                    }
                    var commentNotifs =
                        _context.Notifications.Where(
                            p => p.User.UserId == _userId && p.Article.ArticleId == articleId && p.CommentState);
                    if (commentNotifs.Any())
                    {
                        foreach (Notification commentNotif in commentNotifs)
                        {
                            commentNotif.FavoriteState = true;
                        }
                    }
                    else
                    {
                        Notification newFavoriteNotif = new Notification
                        {
                            User = _user,
                            Article = CurrentArticle,
                            Visited = false,
                            Seen = false,
                            FavoriteState = true,
                            Time = DateTime.Now,
                            ForkedArticleIds = "",
                            Content = "",
                            Identifier = CurrentArticle.Identifier
                        };
                        _context.Notifications.Add(newFavoriteNotif);
                    }
                    break;
            }
            _context.SaveChanges();
        }

        public void RemoveNotificationRecord(string state)
        {
            int articleId = CurrentArticle.ArticleId;
            switch (state)
            {
                case Constant.FavoriteState:
                    var favoriteNotifs = _context.Notifications.Where(
                        p => p.User.UserId == _userId && p.Article.ArticleId == articleId && p.FavoriteState);
                    if (favoriteNotifs.Any())
                    {
                        if (favoriteNotifs.First().CommentState)
                        {
                            foreach (Notification favoriteNotif in favoriteNotifs)
                            {
                                favoriteNotif.FavoriteState = false;
                            }
                        }
                        else
                        {
                            _context.Notifications.RemoveRange(favoriteNotifs);
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        return;
                    }
                    break;

                case Constant.CommentState:
                    return;
                    break;
                   
                case Constant.CreateState:
                    return;
                    break;
            }
        }

        public void SubscribeForkNotificationForEarlierArticles(string identifier)
        {
            if (identifier == Constant.New) return;
            string newArticleId = CurrentArticle.ArticleId.ToString();
            List<int> s = identifier.Split('-').Select(Int32.Parse).ToList();
            int articleId = s[0];
            Article firstArticle = _context.Articles.Include(p=>p.Owner).First(p => p.ArticleInitId == -1 && p.ArticleId==articleId);
            List<Article> articleTail = new List<Article> {firstArticle};
            s.RemoveAt(0);
            int articleIntId = articleId;
            
            foreach (int parallel in s)
            {
                var nextArticle =
                    _context.Articles.Include(p=>p.Owner).First(p => p.ArticleInitId == articleIntId && p.Parallel == parallel);
                articleIntId = nextArticle.ArticleId;
                articleTail.Add(nextArticle);
            }

            List<int> articleIdsFiltered =
                articleTail.Where(p => p.Owner.UserId != _userId)
                    .GroupBy(p => p.Owner.UserId)
                    .Select(p => p.Last())
                    .Select(p => p.ArticleId)
                    .ToList();

            foreach (int id in articleIdsFiltered)
            {
                _context.Notifications.Where(
                p =>
                    p.Article.ArticleId == id && p.User.UserId != _userId && !p.Seen &&
                    (p.CreateState || p.FavoriteState)).ForEach(p =>
                    {
                        p.Forked++;
                        p.Time = DateTime.Now;
                        p.ForkedArticleIds = p.ForkedArticleIds==""
                            ? newArticleId
                            : p.ForkedArticleIds + "|" + newArticleId;
                    });
            }

            _context.SaveChanges();
        }

        public void SubscribeNotification(string action)
        {
            int articleId = CurrentArticle.ArticleId;
            //UserProfile articleOwner = _context.Articles.Include(p=>p.Owner).First(p => p.ArticleId == articleId).Owner;
           
            switch (action)
            {
                case Constant.CommentAction:
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.User.UserId != _userId && !p.Seen).ForEach(p =>
                    {
                        p.Commented++;
                        p.Time = DateTime.Now;
                    });
                    break;
                case Constant.LikeAction:
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.User.UserId != _userId && p.CreateState && !p.Seen)
                        .ForEach(p =>
                        {
                            p.Liked++;
                            p.Time = DateTime.Now;
                        });
                    break;
                case Constant.UnLikeAction:
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.User.UserId != _userId && p.CreateState && !p.Seen)
                        .ForEach(p =>
                        {
                            p.Liked--;
                            p.Time = DateTime.Now;
                        });
                    break;
                case Constant.FavoriteAction:
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.User.UserId != _userId && p.CreateState && !p.Seen)
                        .ForEach(p =>
                        {
                            p.Favorited++;
                            p.Time = DateTime.Now;
                        });
                    break;
                case Constant.UnFavoriteAction:
                    _context.Notifications.Where(p => p.Article.ArticleId == articleId && p.User.UserId != _userId && p.CreateState && !p.Seen)
                        .ForEach(p =>
                        {
                            p.Favorited--;
                            p.Time = DateTime.Now;
                        });
                    break;
            }
            _context.SaveChanges();
        }

        public Tuple<int, NotificationShow[]> GetNotification()
        {
            if (_userId < 0)
            {
                return new Tuple<int, NotificationShow[]>(0, new NotificationShow[] { });
            }
            // removed (DbFunctions.DiffDays(DateTime.Now, p.Time) < 30)
            Notification[] notifications = _context.Notifications.Where(
                p =>
                    p.User.UserId == _userId && (p.Commented > 0 || p.Favorited > 0 || p.Forked > 0 || p.Liked > 0)).Include(p => p.Article).ToArray();
            notifications = notifications.OrderByDescending(p => p.Time).Select(p => p.Seen ? p : new ClassHelper().CreateContent(p)).ToArray();
            int unseen = notifications.Count(p => !p.Seen);
            _context.SaveChanges();
            return new Tuple<int, NotificationShow[]>(unseen, notifications.ToNotificationShows().ToArray());
        }

        public bool SeeNotification()
        {
            var unseenNotification = _context.Notifications.Include(p => p.Article).Include(p => p.User).Where(p => p.User.UserId == _userId && p.Seen == false && p.Content != "");
            foreach (Notification notification in unseenNotification)
            {
                notification.Seen = true;
                Notification newNotification = new Notification
                {
                    User = notification.User,
                    Article = notification.Article,
                    Identifier = notification.Identifier,
                    Time = DateTime.Now,
                    Favorited = 0,
                    Commented = 0,
                    Forked = 0,
                    Liked = 0,
                    Seen = false,
                    Visited = false,
                    ForkedArticleIds = "",
                    CreateState = notification.CreateState,
                    CommentState = notification.CommentState,
                    FavoriteState = notification.FavoriteState,
                    Content = ""
                };
                _context.Notifications.Add(newNotification);
            }
            _context.SaveChanges();
            return true;
        }

        public ArticleUserBase GetArticleUserBaseById(int articleId)
        {
            List<int> userComments = _context.Comments.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userPoints = _context.ArticlePoints.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            List<int> userFavorites = _context.ArticleFavorites.Where(p => p.User.UserId == _userId).Select(p => p.Article.ArticleId).ToList();
            ArticleUserBase output = _context.Articles.Include("Owner").Include("Language").First(p => p.ArticleId == articleId).ArticleToArticleUser(userPoints, userFavorites, userComments, _userId);
            return output;
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
                List<ArticleUserBase> tempArticles = _context.Articles.Include("Owner").Where(p => p.ArticleInitId == sourceId).ArticlesToArticleUsers(userPoints, userFavorites, userComments,_userId).OrderByDescending(p => p.Parallel == i).ThenByDescending(p => p.Point).ToList();
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

        public bool Like(int articleId)
        {
            try
            {
                CurrentArticle = _context.Articles.Include(p=>p.Owner).First(p => p.ArticleId == articleId);
                if (CurrentArticle.Owner.UserId == _userId)
                {
                    return false;
                }
                ArticlePoint articlePoint = new ArticlePoint()
                {
                    Article = CurrentArticle,
                    User = _user
                };
                _context.ArticlePoints.Add(articlePoint);
                CurrentArticle.Point++;
                _context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }

        public bool UnLike(int articleId)
        {
            try{
                CurrentArticle = _context.Articles.Include(p => p.Owner).First(p => p.ArticleId == articleId);
            if (CurrentArticle.Owner.UserId == _userId)
            {
                return false;
            }
            ArticlePoint articlePoint =
    _context.ArticlePoints.First(p => p.Article.ArticleId == articleId && p.User.UserId == _userId);
            _context.ArticlePoints.Remove(articlePoint);
            CurrentArticle.Point--;
            _context.SaveChanges();
            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Star(int articleId)
        {
            try{
                CurrentArticle = _context.Articles.Include(p => p.Owner).First(p => p.ArticleId == articleId);
            if (CurrentArticle.Owner.UserId == _userId)
            {
                return false;
            }
            ArticleFavorite articleFavorite = new ArticleFavorite()
            {
                Article = CurrentArticle,
                User = _user
            };
            _context.ArticleFavorites.Add(articleFavorite);
            CurrentArticle.Favorite++;
            _context.SaveChanges();
            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnStar(int articleId)
        {
            try{
                CurrentArticle = _context.Articles.Include(p => p.Owner).First(p => p.ArticleId == articleId);
            if (CurrentArticle.Owner.UserId == _userId)
            {
                return false;
            }
            ArticleFavorite articleFavorite = _context.ArticleFavorites.First(p => p.Article.ArticleId == articleId && p.User.UserId == _userId);
            _context.ArticleFavorites.Remove(articleFavorite);
            CurrentArticle.Favorite--;
            _context.SaveChanges();
            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Article> GetFavoriteArticle()
        {
            if (_userId < 0)
            {
                return null;
            }
            var queryFavoriteIds =
                _context.ArticleFavorites.Where(p => p.User.UserId == _userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            List<int> favoriteIds = queryFavoriteIds.ToList();
            favoriteIds.Reverse();
            return favoriteIds.Select(p => _context.Articles.First(q => q.ArticleId == p)).ToList();
        }

        public Tuple<Article[], int> GetFirstNFavoriteArticle(int take)
        {
            if (_userId < 0)
            {
                return new Tuple<Article[], int>(new Article[] { }, 0);
            }
            var queryFavoriteIds =
                _context.ArticleFavorites.Where(p => p.User.UserId == _userId)
                    .Include(p => p.Article)
                    .Select(p => p.Article.ArticleId);
            int count = queryFavoriteIds.Count();
            List<int> favoriteIds = queryFavoriteIds.TakeLast(take).ToList();
            favoriteIds.Reverse();
            return new Tuple<Article[], int>(favoriteIds.Select(p => _context.Articles.First(q => q.ArticleId == p)).ToArray(),count);
        }

        public List<Article> GetScriptArticle()
        {
            if (_userId < 0)
            {
                return null;
            }
            var queryArticles = _context.Articles.Where(p => p.Owner.UserId == _userId);
            List<Article> articles = queryArticles.ToList();
            articles.Reverse();
            return articles;
        }

        public Tuple<Article[], int> GetFirstNScriptArticle(int take)
        {
            if (_userId < 0)
            {
                return new Tuple<Article[], int>(new Article[]{}, 0);
            }
            var queryArticles = _context.Articles.Where(p => p.Owner.UserId == _userId);
            int count = queryArticles.Count();
            List<Article> articles = queryArticles.TakeLast(take).ToList();
            articles.Reverse();
            return new Tuple<Article[], int>(articles.ToArray(), count);
        }

        public List<Genre> GetGenreIdsWithMoreThan3()
        {
            return _context.Articles.GroupBy(p => p.Genre).Select(group => new
            {
                Genre = group.Key,
                Count = group.Count()
            }).Where(p => p.Count > 3).Select(p => p.Genre).ToList();
        }

        // key:{Genre, User, Search, Best}
        public List<Article> GetArticles(string key, string value, bool increasing, int from, int take)
        {
            int valueId = 0;
            List<Article> articles = new List<Article>();
            switch (key)
            {
                case "Genre":
                    valueId = Int32.Parse(value);
                    articles =
                        _context.Articles.Where(p => p.ArticleInitId == -1 && p.Genre.GenreId == valueId)
                            .OrderByDescending(p => p.Point)
                            .Skip(from)
                            .Take(take)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
                case "User":
                    valueId = Int32.Parse(value);
                    articles =
                        _context.Articles.Where(p => p.ArticleInitId == -1 && p.Owner.UserId == valueId)
                            .OrderByDescending(p => p.Point)
                            .Skip(from)
                            .Take(take)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
                case "Search":
                    articles =
                        _context.Articles.Where(p => p.ArticleInitId == -1 && (p.Title.ToLower().Contains(value.ToLower()) || p.Text.ToLower().Contains(value.ToLower())))
                            .OrderByDescending(p => p.Point)
                            .Skip(from)
                            .Take(take)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
                case "Best":
                    articles = _context.Articles.Where(p=>p.ArticleInitId == -1)
                            .OrderByDescending(p => p.Point)
                            .Skip(from)
                            .Take(take)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
                case "Language":
                    valueId = Int32.Parse(value);
                    articles =
                        _context.Articles.Where(p => p.ArticleInitId == -1 && p.Language.LanguageId == valueId)
                            .OrderByDescending(p => p.Point)
                            .Skip(from)
                            .Take(take)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
                case "ArticleId":
                    valueId = Int32.Parse(value);
                    articles =
                        _context.Articles.Where(p => p.ArticleId == valueId)
                            .Include(p => p.Owner)
                            .Include(p => p.Genre)
                            .Include(p => p.Language)
                            .ToList();
                    break;
            }
            return articles;
        }

        // key:{Genre, User, Me}
        public int NumberOfArticles(string key, string value)
        {
            int valueId = 0;
            int articleNumber = 0;
            switch (key)
            {
                case "Genre":
                    valueId = Int32.Parse(value);
                    articleNumber =
                        _context.Articles.Count(p => p.ArticleInitId == -1 && p.Genre.GenreId == valueId);
                    break;
                case "User":
                    valueId = Int32.Parse(value);
                    articleNumber =
                        _context.Articles.Count(p => p.ArticleInitId == -1 && p.Owner.UserId == valueId);
                    break;
                case "Me":
                    articleNumber = _context.Articles.Count(p => p.ArticleInitId == -1 && p.Owner.UserId == _userId);
                    break;
            }
            return articleNumber;
        }

        public bool IsInformedUser()
        {
            if (_context.Articles.Count(p => p.Owner.UserId == _userId) > 2)
            {
                return true;
            }
            if (_context.ArticleFavorites.Count(p => p.User.UserId == _userId) > 2)
            {
                return true;
            }
            if (_context.ArticlePoints.Count(p => p.User.UserId == _userId) > 2)
            {
                return true;
            }
            return false;
        }

        public Language[] GetLanguages()
        {
            return _context.Languages.ToArray();
        }

        public bool RemoveArticle(int articleId)
        {
            Article article = _context.Articles.Include("Owner").First(p => p.ArticleId == articleId);
            if (!article.IsLast || article.Owner.UserId != _userId) return false;
            
            //take care of parallel issue
            List <Article> sameLevelArticles = _context.Articles.Where(p => p.ArticleInitId == article.ArticleInitId).ToList();

            if (sameLevelArticles.Count == 1)
            {
                _context.Articles.First(p => p.ArticleId == article.ArticleInitId).IsLast = true;
            }
            else
            {
                foreach (Article sameLevelArticle in sameLevelArticles)
                {
                    if (sameLevelArticle.Parallel > article.Parallel)
                    {
                        sameLevelArticle.Parallel--;
                    }
                }
            }
            _context.Notifications.RemoveRange(_context.Notifications.Where(p => p.Article.ArticleId == articleId));
            _context.ArticleFavorites.RemoveRange(_context.ArticleFavorites.Where(p => p.Article.ArticleId == articleId));
            _context.ArticlePoints.RemoveRange(_context.ArticlePoints.Where(p => p.Article.ArticleId == articleId));
            _context.Comments.RemoveRange(_context.Comments.Where(p => p.Article.ArticleId == articleId));

            _context.SaveChanges();
            _context.Articles.Remove(article);
            _context.SaveChanges();
            return true;
        }
    }
}