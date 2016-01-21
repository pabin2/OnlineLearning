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
        public ActionResult Index(int? logout)
        {
            if (logout == 1)
            {
                @ViewBag.errormessage = "Thanks for login, Hope you enjoyed your time here";
            }
            else if(logout == 2)
            {
                @ViewBag.errormessage = "Session expired,Please login again";
            }
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
                    Session["loggedinusercourseid"] = userDetail[0].courseid.ToString();

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
                @ViewBag.errormessage = "Incorrect username or password";
            }
            else
            {
                @ViewBag.errormessage = "Username/Password cannot be empty";
            }
            return View();
        }

        public ActionResult logout(int? login)
        {

            Session.Abandon();
            FormsAuthentication.SignOut();
            //deleting session and clearing history from browser 
            Session["loggedinusername"] = "";
            Session["loggedinuserschool"] = "";
            Session["loggedinuserschoolid"] = "";
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.RedirectToLoginPage();
            FormsAuthentication.SignOut();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();  
            if (login == 1)
            {
                return RedirectToAction("Index", "Login",new { logout=1 } );
            }
            else if (login == 2)
            {
                return RedirectToAction("Index", "Login", new { logout = 2 });
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
