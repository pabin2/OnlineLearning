using OnlineLearningSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineLearningSystem.Controllers
{
    public class LoginController : Controller
    {

        Sql_connnector sqlconnection = new Sql_connnector();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(logindetail inform)
        {
            if (ModelState.IsValid)
            {
                List<user_info> userDetail = sqlconnection.getuserdetail(inform.username, inform.password, inform.school).ToList();
                if (userDetail.Count == 1)
                {

                    Session["loggedinusername"] = inform.username;
                    Session["loggedinusertype"] = inform.school;
                    Session["loggedinusernameid"] = userDetail[0].id.ToString();
                    Session["loggedinuserschool"] = inform.school;
                    Session["loggedinuserschoolid"] = userDetail[0].schoolid.ToString();


                    if (inform.school == "schooladmin")
                    {
                        return RedirectToAction("Index", "School");
                    }
                    else if (inform.school == "teacher")
                    {
                        return RedirectToAction("Index", "Teacher");
                    }
                    else if (inform.school == "student")
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (inform.school == "superadmin")
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                }
            }
            return View();
        }

        public ActionResult logout()
        {
            //deleting session and clearing history from browser 
            Session["loggedinusername"] = "";
            Session["loggedinuserschool"] = "";
            Session["loggedinuserschoolid"] = "";
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            //FormsAuthentication.SignOut();
            //this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //this.Response.Cache.SetNoStore();          
            return RedirectToAction("Index", "Login");
        }
    }
}
