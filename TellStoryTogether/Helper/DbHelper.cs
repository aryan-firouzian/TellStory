using System.Data.Entity;
using System.Linq;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    
    public static class DbHelper
    {
        readonly static UsersContext UserContext = new UsersContext();
        private static readonly int UserId = WebSecurity.CurrentUserId;

        public static int UserPoints()
        {
            int a = UserContext.UserProfiles.First(p => p.UserId == UserId).UserPoint;

            return a;
        }

        public static Notification[] UserUnseenNotifications()
        {
            return
                UserContext.Notifications.Where(
                    p =>
                        p.User.UserId == UserId && p.Seen == false &&
                        (p.Commented != 0 || p.Favorited != 0 || p.Forked != 0 || p.Liked != 0)).ToArray();
        }

        public static Notification[] UserAllNotifications()
        {
            return
                UserContext.Notifications.Where(
                    p =>
                        p.User.UserId == UserId &&
                        (p.Commented != 0 || p.Favorited != 0 || p.Forked != 0 || p.Liked != 0)).Include(p=>p.Article).ToArray();
        }
    }
}