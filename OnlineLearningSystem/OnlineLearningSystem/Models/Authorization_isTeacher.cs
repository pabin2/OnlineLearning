﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineLearningSystem.Models
{
    public class Authorization_isTeacher : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var loggedinuserschool = HttpContext.Current.Session["loggedinuserschool"];
                if (loggedinuserschool.ToString() == "teacher")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception)
            {
                return false;
            }

        }

    }
}