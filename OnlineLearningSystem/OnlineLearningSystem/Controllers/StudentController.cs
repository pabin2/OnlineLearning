using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLearningSystem.Models;

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
        public ActionResult AssignmentView(int? value)
        {
            try
            {
                List<Assignments> assignmentlist;
                int schoolid = Int32.Parse(Session["loggedinuserschoolid"].ToString());
                int userid = Int32.Parse(Session["loggedinusernameid"].ToString());
                assignmentlist = sql.Displayassignment(schoolid, userid)
                    .Distinct(new AssignmentComparer()).OrderBy(m => m.name).ToList();
                return View(assignmentlist);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult MessageView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Submitted()
        {
            return RedirectToAction("AssignmentView", "Student");
        }
        public ActionResult TeacherView()
        {
            return View();
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
