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
        Sql_connnector sql = new Sql_connnector();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult School(int? sucess)
        {
            List<School> myschool = new List<School>();
            myschool = sql.Getallschool().ToList();
            if (sucess==1)
            {
                @ViewBag.message = "Successfully Added";
            }
            else if(sucess == 0)
            {
                @ViewBag.message = "Failed Adding";
            }
            return View(myschool);
        }

        //insert school
        [HttpPost]
        public int Addschool(School schooldetail)
        {
            var result = sql.InsertSchool(schooldetail);
            return result;

        }

    }
}
