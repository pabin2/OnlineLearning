using CrystalDecisions.CrystalReports.Engine;
using OnlineLearningSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult School(int? success)
        {
            List<School> myschool = new List<School>();
            myschool = sql.Getallschool().ToList();
            if (success == 1)
            {
                @ViewBag.message = "Successfully Added";
            }
            else if (success == 0)
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

        //addming admin for school
        [HttpGet]
        public ActionResult Addschooladmin(int? schoolid)
        {
            return View();

        }

        [HttpGet]
        public ActionResult Report()
        {
            List<user_info> alluser;
            alluser = sql.displayteacher(1, false, 1).ToList();
            return View(alluser);
        }
        public ActionResult Exportreport()
        {
            List<user_info> alluser;
            alluser = sql.displayteacher(1, false, 1).ToList();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"),"Myreport.rpt"));
            rd.SetDataSource(alluser);
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "report.pdf");
        }

        [HttpPost]
        public ActionResult Addschooladmin(user_info userinfo)
        {
            var result = sql.Insertuser(userinfo, "schooladmin");
            if (result == 1)
                return RedirectToAction("School", "SuperAdmin", new { success = 1 });
            else
                return RedirectToAction("School", "SuperAdmin", new { success = 0 });

        }

        [HttpPost]
        public int CheckExistingUser(user_info userinfo)
        {
            var result = sql.getuserdetail(userinfo.username);
            return result;
        }

        [HttpGet]
        [ActionName("Backup")]
        public ActionResult Backup(int? success)
        {
            if (success == 1)
            {
                @ViewBag.message = "Successfully backup completed";
            }
            else if(success==2)
            {
                @ViewBag.message = "Successfully restored completed";   
            }

            return View();
        }

        [HttpPost]
        public ActionResult BackupPost()
        {
            var result = sql.Backup();
            return RedirectToAction("Backup", "SuperAdmin", new { success = 1 });
        }
        [HttpPost]
        public ActionResult Restore()
        {
            var result = sql.Restore();
            return RedirectToAction("Backup", "SuperAdmin", new { success = 2 });
        }
    }
}
