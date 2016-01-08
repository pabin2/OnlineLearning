using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLearningSystem.Models
{
    public class Authorization_isSuperAdmin : AuthorizeAttribute
    {
         protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var loggedinuserschool = HttpContext.Current.Session["loggedinuserschool"];
                if (loggedinuserschool.ToString() == "superadmin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception exc)
            {
                return false;
            }

        }

    }
}