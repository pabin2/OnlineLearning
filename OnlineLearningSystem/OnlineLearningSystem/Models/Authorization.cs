using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineLearningSystem.Models
{
    public class Authorization : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var loggedinuserid = HttpContext.Current.Session["loggedinusername"];
                if (loggedinuserid == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            catch (Exception exc)
            {
                throw exc;
            }

        }

    }
}