using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TellStoryTogether.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Filters;
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