using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLearningSystem.Models;
using PagedList;
using PagedList.Mvc;

namespace OnlineLearningSystem.Controllers
{
    [Authorization_isStudent]
    [Authorization]

    public class StudentController : Controller
    {
        //
        // GET: /Student/

        Sql_connnector sql = new Sql_connnector();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AssignmentView(int? value,int? page)
        {
            try
            {
                List<Assignments> assignmentlist;
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                assignmentlist = sql.Displayassignment(schoolid, userid)
                    .Distinct(new AssignmentComparer()).OrderBy(m => m.name).ToList();
                IPagedList<Assignments> assignmentlist1 = assignmentlist.ToPagedList(page ?? 1, 4);
                return View(assignmentlist1);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public ActionResult MessageView(string search, string sortBy, int? page)
        {
            List<message> message = new List<message>();
            //triggering asc and desc
            ViewBag.sortByParam = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            string usertype = Session["loggedinusertype"].ToString();
            if (search == null)
            {

                message = sql.displaymessage(schoolid, userid, usertype).ToList();
            }
            else
            {
                message = sql.displaymessage(schoolid, userid, usertype).Where(m => m.sender.StartsWith(search)).ToList();


            }
            IPagedList<message> message1 = message.ToPagedList(page ?? 1, 4);
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
            return View(message1);
        }
        [HttpGet]
        public ActionResult Submitted()
        {
            return RedirectToAction("AssignmentView", "Student");
        }
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
            teacherlist.Add(new user_info());
            IPagedList<user_info> teacherlist1 = teacherlist.ToPagedList(page ?? 1, 5);
            return View(teacherlist1);

        }
        [HttpGet]
        public ActionResult AssignmentViewDetail(string id)
        {
            if (id == null)
            {
                return RedirectToAction("AssignmentView", "Student");
            }
            List<Assignments> assignmentdetaillist;
            int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());

            int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
            assignmentdetaillist = sql.Displayassignmentforstudent(schoolid, userid,id).Where(m => m.name == id).ToList();
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/uploads/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles(id + "*.*");
            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }
            ViewBag.files = items;

            return View(assignmentdetaillist);
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
        //sentmessage
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

        [HttpPost]
        public int AssignmentViewDetail(StudentResponse response)
        {
            int result;
            try
            {
                result = sql.StudentResponse(response);
                return result;
            }
            catch (Exception)
            {
                result = 0;
                return result;
            }

        }
        [HttpPost]
        public int SubmitAssignment(int assignmentid)
        {
            int result;
            try
            {
                result = sql.SubmitAssignment(assignmentid);
                return result;
            }
            catch (Exception)
            {
                result = 0;
                return result;
            }

        }
        [HttpPost]
        public int Reviewteacher(Review reviewdetail)
        {
            int result;
            try
            {
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                result = sql.Insertreviewteacher(userid,reviewdetail.teacherid,reviewdetail.stars);
                return result;
            }
            catch (Exception)
            {
                result = 0;
                return result;
            }

        }
        [HttpGet]
        public ActionResult SubjectView()
        {
            return View();
        
        }

        [HttpGet]
        public ActionResult Detailcourse()
        {
            List<Coursedetail> coursedetail;
            coursedetail = sql.Detailcourse().ToList();
            return View(coursedetail);
        }
    }

    public class AssignmentComparer : IEqualityComparer<Assignments>
    {
        public bool Equals(Assignments x, Assignments y)
        {
            return x.name.ToLower() == y.name.ToLower();
        }

        public int GetHashCode(Assignments obj)
        {
            return base.GetHashCode();
        }
    }

    

}
