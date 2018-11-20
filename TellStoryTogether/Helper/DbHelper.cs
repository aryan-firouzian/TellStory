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

        public static Notification[] UserNotifications()
        {
            return UserContext.Notifications.Where(p => p.User.UserId == UserId && p.Seen == false).ToArray();
        }
    }
}