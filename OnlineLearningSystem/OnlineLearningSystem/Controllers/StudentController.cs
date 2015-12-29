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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AssignmentView()
        {
            return View();
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
}
