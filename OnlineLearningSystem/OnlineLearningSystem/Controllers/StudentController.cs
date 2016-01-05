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
                assignmentlist = sql.Displayassignmentforstudent(schoolid, userid)
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
        public ActionResult TeacherView()
        {
            return View();
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
