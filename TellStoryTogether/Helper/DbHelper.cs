using System.Data.Entity;
using System.Linq;
using System.Web;
using TellStoryTogether.Filters;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    public static class DbHelper
    {
        readonly static UsersContext UserContext = new UsersContext();
        public static int UserPoints()
        {
            int userId = WebSecurity.CurrentUserId;
            int a = UserContext.UserProfiles.First(p => p.UserId == userId).UserPoint;

            return a;
        }

        public static Notification[] UserUnseenNotifications()
        {
            int userId = WebSecurity.CurrentUserId;
            return
                UserContext.Notifications.Where(
                    p =>
                        p.User.UserId == userId && p.Seen == false &&
                        (p.Commented != 0 || p.Favorited != 0 || p.Forked != 0 || p.Liked != 0)).ToArray();
        }

        public static Notification[] UserAllNotifications()
        {
            int userId = WebSecurity.CurrentUserId;
            return
                UserContext.Notifications.Where(
                    p =>
                        p.User.UserId == userId &&
                        (p.Commented != 0 || p.Favorited != 0 || p.Forked != 0 || p.Liked != 0)).Include(p=>p.Article).ToArray();
        }
    }
}