using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellStoryTogether.Models;

namespace TellStoryTogether.Helper
{
    public class MyTimeSpan
    {
        public string TillNow(DateTime eventDateTime)
        {
            const int second = 1;
            const int minute = 60*second;
            const int hour = 60*minute;
            const int day = 24*hour;
            const int month = 30*day;

            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - eventDateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1*minute)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2*minute)
                return "a minute ago";

            if (delta < 45*minute)
                return ts.Minutes + " minutes ago";

            if (delta < 90*minute)
                return "an hour ago";

            if (delta < 24*hour)
                return ts.Hours + " hours ago";

            if (delta < 48*hour)
                return "yesterday";

            if (delta < 30*day)
                return ts.Days + " days ago";

            if (delta < 12*month)
            {
                int months = Convert.ToInt32(Math.Floor((double) ts.Days/30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            int years = Convert.ToInt32(Math.Floor((double) ts.Days/365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

    }
}