using System.Linq;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Helper
{
    
    public static class DbHelper
    {
        readonly static UsersContext _userContext = new UsersContext();

        public static int UserPoints()
        {
            int a = _userContext.UserProfiles.First(p => p.UserId == WebSecurity.CurrentUserId).UserPoint;

            return a;
        }
    }
}