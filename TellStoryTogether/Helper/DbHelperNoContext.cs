using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using TellStoryTogether.Models;
using WebGrease.Css.Extensions;

namespace TellStoryTogether.Helper
{
    public static class DbHelperNoContext
    {
        public static void AddNotificationRecord(UsersContext context, ref UserProfile user, ref Article article, string state)
        {
            int userId = user.UserId;
            int articleId = article.ArticleId;
            if(context.Notifications.Any(p=>p.User.UserId == userId && p.Article.ArticleId == articleId && p.State=="All"))
            {

            }
            else
            {
                Notification notification = new Notification
                {
                    User = user,
                    Article = article,
                    Seen = false,
                    State = state,
                    Time = DateTime.Now,
                    Identifier = article.Identifier
                    
                };
                context.Notifications.Add(notification);
                context.SaveChanges();
            }

        }

        public static void RemoveNotificationRecord(UsersContext context, int userId, int articleId, string state)
        {
            IQueryable<Notification> notification =
                    context.Notifications.Where(
                        p => p.User.UserId == userId && p.Article.ArticleId == articleId && p.State == state);
            context.Notifications.RemoveRange(notification);
            context.SaveChanges();
        }

        public static void SubscribeNotification(UsersContext context, int articleId, string action)
        {
            switch (action)
            {
                case "Comment":
                    context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false).ForEach(p => p.Commented++);
                    context.SaveChanges();
                    break;
                case "Like":
                    context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Liked++);
                    context.SaveChanges();
                    break;
                case "UnLike":
                    context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Liked--);
                    context.SaveChanges();
                    break;
                case "Favorite":
                    context.Notifications.Where(p => p.Article.ArticleId == articleId && p.Seen == false && p.State == "All").ForEach(p => p.Favorited++);
                    context.SaveChanges();
                    break;
                case "UnFavorite":
                    context.Notifications.Where(p=>p.Article.ArticleId==articleId&&p.Seen==false&&p.State=="All").ForEach(p=>p.Favorited--);
                    context.SaveChanges();
                    break;
            }
        }

        public static void SubscribeForkNotification(UsersContext context, string identifier)
        {
            if (identifier=="new") return;
            List<int> s = identifier.Split('-').Select(Int32.Parse).ToList();
            int articleId = s[0];
            context.Notifications.Where(
                p =>
                    p.Article.ArticleId == articleId && p.Seen == false &&
                    (p.State == "All" || p.State == "Favorite")).ForEach(p => p.Forked++);
            s.RemoveAt(0);
            int articleIntId = articleId;
            foreach (int parallel in s)
            {
                var parallel1 = parallel;
                var nextArticle =
                    context.Articles.First(p => p.ArticleInitId == articleIntId && p.Parallel == parallel1);
                context.Notifications.Where(p => p.Article.ArticleId == nextArticle.ArticleId && p.Seen == false)
                    .ForEach(p => p.Forked++);
                articleIntId = nextArticle.ArticleId;
            }
            context.SaveChanges();
        }
    }
}