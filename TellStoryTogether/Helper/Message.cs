using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellStoryTogether.Helper
{
    public static class Message
    {

        public static string[] UnAuthenticated
        {
            get
            {
                return new[]
                {
                    "rejected",
                    "The request from an unauthenticated user. Log in or Register!"
                };
            }
        }

        public static string[] Added
        {
            get
            {
                return new[]
                {
                    "added",
                    "no message"
                };
            }
        }

        public static string[] Removed
        {
            get
            {
                return new[]
                {
                    "removed",
                    "no message"
                };
            }
        }

        public static string[] ServerRejected(Exception e)
        {
            //store e.Message somewhere.
            return new[]
            {
                "rejected",
                "no message"
            };

        }

        public static string[] AddedWithMessage(string message)
        {
            return new[]
            {
                "added",
                message
            };

        }



    }
}