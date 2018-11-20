using System.Linq;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    
    public static class DbHelper
    {
        readonly static UsersContext UserContext = new UsersContext();

        public static int UserPoints()
        {
            int a = UserContext.UserProfiles.First(p => p.UserId == WebSecurity.CurrentUserId).UserPoint;

            return a;
        }
    }
}