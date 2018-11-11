using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

    public class ArticleUserBase:Article
    {
        public bool Pointed { get; set; }

        public bool Favorited { get; set; }

        public bool Commented { get; set; }
    }

    public static class LinqExtension
    {
        public static IEnumerable<CommentTime> ChangeTime(this IEnumerable<Comment> source)
        {
            MyTimeSpan myTimeSpan = new MyTimeSpan();
            List<CommentTime> commentTimes = new List<CommentTime>();
            foreach (Comment comment in source)
            {
                CommentTime commentTime = new CommentTime
                {
                    ArticleId = comment.ArticleId,
                    Content = comment.Content,
                    CommentId = comment.CommentId,
                    User = comment.User,
                    Time = myTimeSpan.TillNow(comment.Time)
                };
                commentTimes.Add(commentTime);
            }
            return commentTimes;
        }

        public static IEnumerable<ArticleUserBase> ChangeTime(this IEnumerable<Article> source, List<ArticlePoint> articlePoints, List<ArticleFavorite> articleFavorites)
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
                    Selected = article.Selected,
                    Point = article.Point,
                    Seen = article.Seen,
                    Favorite = article.Favorite,
                    Comment = article.Comment,
                    PictureUrl = article.PictureUrl,
                    Time = article.Time,
                    Genre = article.Genre,
                    Language = article.Language,
                    MinChar = article.MinChar,
                    MaxChar = article.MaxChar
                };
                if (articlePoints.Any(p => p.Article.ArticleId == article.ArticleId))
                {
                    articleUserBase.Pointed = true;
                }
                if (articleFavorites.Any(p => p.Article.ArticleId == article.ArticleId))
                {
                    articleUserBase.Favorited = true;
                }
                articleUserBases.Add(articleUserBase);
            }
            return articleUserBases;
        }  
    }
}