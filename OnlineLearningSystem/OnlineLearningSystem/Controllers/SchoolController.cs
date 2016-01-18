using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLearningSystem.Models;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
namespace OnlineLearningSystem.Controllers
{
    [Authorization]
    [Authorization_isSchool]
    public class SchoolController : Controller
    {
        string pagelist = ConfigurationManager.AppSettings["pagelist"];
        Sql_connnector sql = new Sql_connnector();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StudentView(string search, int? page)
        {
            if (search == null)
            {

                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).ToList();
                //added blank student so that while adding student , they dont appears there
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }
            else
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                //converted list to IpagedLIst in order to pass IpagedList in model for pagination
                List<user_info> studentlist = sql.displaystudent(schoolid).Where(t => t.username.StartsWith(search)).ToList();
                //added blank student so that while adding student , they dont appears there
                IPagedList<user_info> studentlist1 = studentlist.ToPagedList(page ?? 1, 4);
                return View(studentlist1);
            }

        }

        [HttpPost]
        public int StudentView(user_info studentdetail)
        {

            var a = sql.Insertuser(studentdetail, "student");
            return a;

        }
        [HttpGet]
        public ActionResult MessageView(string search, string sortBy,int? page)
        {
            List<message> message = new List<message>();
            //triggering asc and desc
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            string usertype = Session["loggedinusertype"].ToString();
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            if (search == null)
            {


                message = sql.displaymessage(schoolid, userid, usertype).ToList();
            }
            else
            {
                message = sql.displaymessage(schoolid, userid, usertype).Where(m => m.sender.StartsWith(search)).ToList();


            }

            IPagedList<message> messagelist = message.ToPagedList(page ?? 1, Int32.Parse(pagelist));
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
            return View(messagelist);
        }
        [HttpGet]
        public ActionResult TeacherView(string search, int? page, string sortBy)
        {

            List<user_info> teacherlist;
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            if (search == null)
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                teacherlist = sql.displayteacher(schoolid, false, 0).OrderBy(s => s.username).ToList();

            }
            else
            {
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                teacherlist = sql.displayteacher(schoolid, false, 0).Where(t => t.username.StartsWith(search)).ToList();
            }

            switch (sortBy)
            {
                case "name_desc":
                    teacherlist = teacherlist.OrderByDescending(s => s.username).ToList();
                    //return View(teacherlist1);
                    break;
                case "name_asc":
                    teacherlist = teacherlist.OrderBy(s => s.username).ToList();
                    break;
                default:
                    teacherlist = teacherlist.OrderBy(s => s.username).ToList();
                    break;
            }

            IPagedList<user_info> teacherlist1 = teacherlist.ToPagedList(page ?? 1, Int32.Parse(pagelist));
            return View(teacherlist1);

        }
        [HttpPost]
        public int TeacherView(user_info teacherdetail)
        {
            var result = sql.Insertuser(teacherdetail, "teacher");
            return result;
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

        public ActionResult EditStudent()
        {
            return View();
        }
        //[HttpGet]
        //public ActionResult EditTeacher(int id)
        //{
        //    int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
        //    List<user_info> editTeacher = new List<user_info>();
        //    editTeacher = sql.displayteacher(schoolid, true, id).ToList();
        //    return View("editTeacher", editTeacher);
        //}
        [HttpPost]
        public int EditTeacher(user_info userinfo)
        {
            int i = sql.editTeacher(userinfo);
            return i;
        }
        [HttpPost]
        public int DeleteTeacher(user_info userinfo)
        {
            int i = sql.deleteTeacher(userinfo.id);
            return i;
        }


        [HttpPost]
        public int CheckExistingUser(user_info userinfo)
        {
            var result = sql.getuserdetail(userinfo.username);
            return result;
        }
        [HttpGet]
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
    }
}
