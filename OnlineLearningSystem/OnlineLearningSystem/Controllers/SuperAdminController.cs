using OnlineLearningSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineLearningSystem.Controllers
{
    [Authorization]
    [Authorization_isSuperAdmin]
    public class SuperAdminController : Controller
    {
        //
        // GET: /SuperAdmin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult School()
        {
            return View();
        }

    }
}
