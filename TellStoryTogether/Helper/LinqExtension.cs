using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using TellStoryTogether.Models;

namespace TellStoryTogether.Helper
{
    public class CommentTime
    {
        public int CommentId { get; set; }

        public Article ArticleId { get; set; }

        public UserProfile User { get; set; }

        public string Content { get; set; }

        public string Time { get; set; }
    }

    public class ArticleUserBase
    {
        public int ArticleId { get; set; }

        public int ArticleInitId { get; set; }

        public string Identifier { get; set; }

        public string Title { get; set; }

        public int Serial { get; set; }

        public int Parallel { get; set; }

        public string Text { get; set; }

        public UserProfile Owner { get; set; }

        public int Point { get; set; }

        public int Seen { get; set; }

        public int Favorite { get; set; }

        public int Comment { get; set; }

        public string PictureUrl { get; set; }

        public DateTime Time { get; set; }

        public Genre Genre { get; set; }

        public Language Language { get; set; }

        public int MinChar { get; set; }

        public int MaxChar { get; set; }

        public bool Pointed { get; set; }

        public bool Favorited { get; set; }

        public bool Commented { get; set; }

        public bool MyArticle { get; set; }

        public bool LastArticle { get; set; }
    }

    public class NotificationShow
    {
        public int NotificationId { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleText { get; set; }

        public bool Bold { get; set; }

        public string Content { get; set; }

        public string Identifier { get; set; }
    }

    public class HomeFeed
    {
        public string PropKey { get; set; }
        public string PropValue { get; set; }
        public string PropTitle { get; set; }
        public string PropDescription { get; set; }
        public string PropDescription2 { get; set; }
        public string PropDescription3 { get; set; }
        public int Take { get; set; }
        public int From { get; set; }
    }

    public static class LinqExtension
    {
        public static IEnumerable<CommentTime> ChangeTime(this IEnumerable<Comment> source)
        {
            ClassHelper classHelper = new ClassHelper();
            List<CommentTime> commentTimes = new List<CommentTime>();
            foreach (Comment comment in source)
            {
                CommentTime commentTime = new CommentTime
                {
                    ArticleId = comment.Article,
                    Content = comment.Content,
                    CommentId = comment.CommentId,
                    User = comment.User,
                    Time = classHelper.TillNow(comment.Time)
                };
                commentTimes.Add(commentTime);
            }
            return commentTimes;
        }

        public static IEnumerable<ArticleUserBase> ArticlesToArticleUsers(this IEnumerable<Article> source,
            List<int> articlePoints, List<int> articleFavorites, List<int> comments, int userId)
        {
            List<ArticleUserBase> articleUserBases = new List<ArticleUserBase>();
            foreach (Article article in source)
            {
                ArticleUserBase articleUserBase = new ArticleUserBase()
                {
                    ArticleId = article.ArticleId,
                    ArticleInitId = article.ArticleInitId,
                    Identifier = article.Identifier,
                    Title = article.Title,
                    Serial = article.Serial,
                    Parallel = article.Parallel,
                    Text = article.Text,
                    Owner = article.Owner,
                    Point = article.Point,
                    Seen = article.Seen,
                    Favorite = article.Favorite,
                    Comment = article.Comment,
                    PictureUrl = article.PictureUrl,
                    Time = article.Time,
                    Genre = article.Genre,
                    Language = article.Language,
                    MinChar = article.MinChar,
                    MaxChar = article.MaxChar,
                    LastArticle = article.IsLast
                };
                articleUserBase.MyArticle = article.Owner.UserId == userId;
                if (articlePoints != null && articlePoints.Any(p => p == article.ArticleId))
                {
                    articleUserBase.Pointed = true;
                }
                if (articleFavorites != null && articleFavorites.Any(p => p == article.ArticleId))
                {
                    articleUserBase.Favorited = true;
                }
                if (comments != null && comments.Any(p => p == article.ArticleId))
                {
                    articleUserBase.Commented = true;
                }
                articleUserBases.Add(articleUserBase);
            }
            return articleUserBases;
        }

        public static ArticleUserBase ArticleToArticleUser(this Article source, List<int> articlePoints,
            List<int> articleFavorites, List<int> comments, int userId)
        {
            ArticleUserBase articleUserBase = new ArticleUserBase()
            {
                ArticleId = source.ArticleId,
                ArticleInitId = source.ArticleInitId,
                Identifier = source.Identifier,
                Title = source.Title,
                Serial = source.Serial,
                Parallel = source.Parallel,
                Text = source.Text,
                Owner = source.Owner,
                Point = source.Point,
                Seen = source.Seen,
                Favorite = source.Favorite,
                Comment = source.Comment,
                PictureUrl = source.PictureUrl,
                Time = source.Time,
                Genre = source.Genre,
                Language = source.Language,
                MinChar = source.MinChar,
                MaxChar = source.MaxChar,
                LastArticle = source.IsLast
            };
            articleUserBase.MyArticle = source.Owner.UserId == userId;

            if (articlePoints != null && articlePoints.Any(p => p == source.ArticleId))
            {
                articleUserBase.Pointed = true;
            }
            if (articleFavorites != null && articleFavorites.Any(p => p == source.ArticleId))
            {
                articleUserBase.Favorited = true;
            }
            if (comments != null && comments.Any(p => p == source.ArticleId))
            {
                articleUserBase.Commented = true;
            }
            return articleUserBase;
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            IEnumerable<T> enumerable = source as T[] ?? source.ToArray();
            return enumerable.Skip(Math.Max(0, enumerable.Count() - n));
        }

        public static IEnumerable<NotificationShow> ToNotificationShows(this IEnumerable<Notification> source)
        {
            bool thereAreUnseen = source.Any(p => p.Seen == false);
            List<NotificationShow> notificationShows = new List<NotificationShow>();
            foreach (Notification notification in source)
            {
                NotificationShow notificationShow = new NotificationShow();
                notificationShow.NotificationId = notification.NotificationId;
                notificationShow.ArticleTitle = notification.Article.Title;
                notificationShow.ArticleText = notification.Article.Text;
                notificationShow.Identifier = notification.Identifier;
                notificationShow.Content = notification.Content;
                if (thereAreUnseen)
                {
                    if (notification.Seen == false)
                    {
                        notificationShow.Bold = true;
                    }
                }
                else
                {
                    if ((DateTime.Now - notification.Time).TotalDays < 2)
                    {
                        notificationShow.Bold = true;
                    }
                }
                notificationShows.Add(notificationShow);
            }
            return notificationShows;
        }
    }
}