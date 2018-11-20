using System;
using System.Linq;
using System.Web.UI;
using TellStoryTogether.Models;

namespace TellStoryTogether.Helper
{
    public static class DbHelperNoContext
    {
        public static void AddNotificationRecord(UsersContext context, UserProfile user, Article newArticle, string state)
        {
            Notification notification = new Notification
            {
                User = user,
                Article = newArticle,
                Seen = false,
                State = "All",
                Time = DateTime.Now
            };
            context.Notifications.Add(notification);
            context.SaveChanges();
        }

        public static void RemoveNotificationRecord(UsersContext context, int userId, int articleId, string state )
        {
            Notification notification =
                    context.Notifications.First(
                        p => p.User.UserId == userId && p.Article.ArticleId == articleId && p.State == state);
            context.Notifications.Remove(notification);
            context.SaveChanges();
        }
    }
}