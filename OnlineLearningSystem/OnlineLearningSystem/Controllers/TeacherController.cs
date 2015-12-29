using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLearningSystem.Models;
using System.IO;
namespace OnlineLearningSystem.Controllers
{
    public class TeacherController : Controller
    {
        Sql_connnector sql = new Sql_connnector();

        // GET: /Teacher/
        [Authorization]
        [Authorization_isTeacher]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AssignmentView()
        {
            List<Assignments> assignmentlist;

            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());

            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            assignmentlist = sql.displayassignment(schoolid, userid).ToList();


            return View(assignmentlist);
        }

        //[HttpPost]
        //public ActionResult AssignmentView(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // extract only the filename
        //        var fileName = Path.GetFileName(file.FileName);
        //        // store the file inside ~/App_Data/uploads folder
        //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
        //        file.SaveAs(path);
        //    }
        //    return RedirectToAction("AssignmentView", "Teacher");
        //}
        [HttpPost]
        public ActionResult AssignmentView(Assignments assignment)
        {
            return RedirectToAction("AssignmentView", "Teacher");
        }

        [HttpGet]
        public ActionResult MessageView(string search, string sortBy)
        {
            List<message> message = new List<message>();
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            if (search == null)
            {

                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                message = sql.displaymessage(schoolid, userid).ToList();
            }
            else
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                message = sql.displaymessage(schoolid, userid).Where(m => m.sender.StartsWith(search)).ToList();


            }
            switch (sortBy)
            {
                case "name_desc":
                    message = message.OrderByDescending(s => s.sender).ToList();
                    break;
                case "name_asc":
                    message = message.OrderBy(s => s.sender).ToList();
                    break;
                default:
                    message = message.OrderBy(s => s.sender).ToList();
                    break;
            }
            return View(message);
        }
        public ActionResult SchoolAdmin()
        {
            return View();
        }

        [HttpPost]
        public int MessageView(message message_info)
        {
            int a = 0;
            int i = 0;
            List<string> listofall = new List<string>();
            if (message_info.usertype == "All")
            {
                listofall.Add("teacher");
                listofall.Add("student");
            }
            else
            {
                listofall.Add(message_info.usertype);
            }
            while (i < listofall.Count)
            {
                string sender = Session["loggedinusername"].ToString();
                string schoolid = Session["loggedinuserschoolid"].ToString();
                DateTime date = DateTime.Now;
                a = sql.insertMessage(message_info, date, sender, schoolid, listofall[i]);
                i++;
            }
            return a;

        }
        public ActionResult SentMessage(string search, string sortBy)
        {
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            List<message> message = new List<message>();
            if (search == null)
            {
                string username = Session["loggedinusername"].ToString();
                message = sql.displaysendmessage(username).ToList();

            }
            else
            {
                string username = Session["loggedinusername"].ToString();
                message = sql.displaysendmessage(username).Where(m => m.receiver_name.StartsWith(search)).ToList(); ;

            }
            switch (sortBy)
            {
                case "name_desc":
                    message = message.OrderByDescending(s => s.receiver_name).ToList();
                    break;
                case "name_asc":
                    message = message.OrderBy(s => s.receiver_name).ToList();
                    break;
                default:
                    message = message.OrderBy(s => s.receiver_name).ToList();
                    break;
            }
            return View(message);
        }

        public ActionResult Downloads()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/uploads/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }
            return View(items);
        }

        public FileResult Download(string ImageName)
        {
            return File("~/App_Data/uploads/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
    }
}
