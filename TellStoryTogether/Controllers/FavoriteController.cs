using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Filters;
using TellStoryTogether.Helper;
using TellStoryTogether.Models;
using WebMatrix.WebData;

namespace TellStoryTogether.Controllers
{
    public class FavoriteController : Controller
    {
        // GET: /Favorite/
        public ActionResult Index()
        {
            DAL dal = new DAL(User.Identity.Name);
            return View(dal.GetFavoriteArticle());
        }
    }
}
