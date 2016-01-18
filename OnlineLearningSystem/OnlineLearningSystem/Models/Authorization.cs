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
                    // Validate cookie
                    //var cookieVal = HttpContext.Current.Request.Cookies["_login"];
                    //if (cookieVal.Value != null)
                    //{
                    //    var inform = cookieVal.Value.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
                    //    HttpContext.Current.Session["loggedinusername"] = inform[0];
                    //    HttpContext.Current.Session["loggedinusertype"] = inform[1];
                    //    HttpContext.Current.Session["loggedinusernameid"] = inform[2];
                    //    HttpContext.Current.Session["loggedinuserschool"] = inform[3];
                    //    HttpContext.Current.Session["loggedinuserschoolid"] = inform[4];
                    //    return true;
                    //}
                    return false;
                }
                else
                {
                    return true;
                }

                
            }

            catch (Exception exc)
            {
                return false;
            }

        }

    }
}