using System;
using System.Linq;
using System.Web.UI;
using TellStoryTogether.Models;

namespace TellStoryTogether.Helper
{
    public static class DbHelperNoContext
    {
        public static void AddNotificationRecord(UsersContext context, UserProfile user, Article article, string state)
        {
            if(context.Notifications.Any(p=>p.User == user && p.Article == article && p.State=="All"))
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
                    Time = DateTime.Now
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
    }
}