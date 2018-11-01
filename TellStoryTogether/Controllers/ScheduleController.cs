using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellStoryTogether.Models;

namespace TellStoryTogether.Controllers
{
    public class ScheduleController : Controller
    {
        readonly UsersContext _userContext = new UsersContext();
        //
        // GET: /Schedule/
        public ActionResult Index(string identifier)
        {
            string result = "";
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (identifier == "1")
                    {
                        // modify maxChar and minChar for all items in article table
                        /*var all = _userContext.Articles.ToList();
                        foreach (Article article in all)
                        {
                            article.MinChar = 140;
                            article.MaxChar = 2000;
                        }
                        _userContext.SaveChanges();*/
                        result = "done";
                    }
                    else
                    {
                        result = "incorrect identifier";
                    }
                    
                }
            }
            catch (Exception e)
            {
                result = "unhandled exception";
            }
            ViewBag.result = result;
            return View(); 
        }

    }
}
