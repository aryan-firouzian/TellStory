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
        // GET: /Schedule?identifier=1
        public ActionResult Index(string identifier)
        {
            string result = "";
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (identifier == "1")
                    {
                        /*List<Comment> all = _userContext.Comments.ToList();
                        foreach (Comment comment in all)
                        {
                            comment.Time = DateTime.Now;

                        }
                        _userContext.SaveChanges();
                        result = "done";*/
                    }
                    else
                    {
                        result = "incorrect identifier";
                    }
                    
                }
            }
            catch (Exception)
            {
                result = "unhandled exception";
            }
            ViewBag.result = result;
            return View(); 
        }

    }
}
